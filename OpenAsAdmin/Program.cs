using Microsoft.Win32;
using System.CommandLine;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Windows.Win32;
using Windows.Win32.UI.Shell;

namespace OpenAsAdmin
{
    internal class Program
    {
        private const string OpenAdAdminKey = @"*\shell\OpenAsAdmin";

        private static bool IsRunningAsAdmin()
        {
            try
            {
                using var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        private static int RunAsAdmin()
        {
            var args = Environment.GetCommandLineArgs();
            var psi = new ProcessStartInfo
            {
                FileName = Environment.ProcessPath,
                UseShellExecute = true,
                Verb = "runas",
                WorkingDirectory = Environment.CurrentDirectory,
            };
            for (int i = 1; i < args.Length; i++)
            {
                psi.ArgumentList.Add(args[i]);
            }
            using var process = Process.Start(psi);
            if (process != null)
            {
                process.WaitForExit();
                return process.ExitCode;
            }
            return -1;
        }

        private static Command BuildInstallCommand()
        {
            var cmd = new Command("install", "Install to the context menu.");
            var name = new Option<string>("--name", "-n") { Description = "The name of the context menu.", Required = true };
            cmd.Options.Add(name);
            cmd.SetAction(p => Install(p.GetRequiredValue(name)));
            return cmd;
        }

        private static Command BuildOpenCommand()
        {
            var cmd = new Command("open", "Open the specified file use associated application with administrator privilige.");
            var file = new Option<string>("--file", "-f") { Description = "The file to open.", Required = true}.AcceptLegalFilePathsOnly();
            cmd.Options.Add(file);
            cmd.SetAction(p => Open(p.GetRequiredValue(file)));
            return cmd;
        }

        private static Command BuildUninstallCommand()
        {
            var cmd = new Command("uninstall", "Uninstall from the context menu.");
            cmd.SetAction(p => Uninstall());
            return cmd;
        }

        private static int Install(string name)
        {
            if (!IsRunningAsAdmin())
            {
                return RunAsAdmin();
            }

            try
            {
                var executable = Environment.ProcessPath!;

                using var key = Registry.ClassesRoot.CreateSubKey(OpenAdAdminKey);
                if (key == null)
                {
                    Console.WriteLine("Can't create registry key.");
                    return -1;
                }

                key.SetValue(null, name);
                key.SetValue("Icon", executable);
                key.SetValue("DelegateExecute", "");

                using var cmdKey = key.CreateSubKey("command");
                cmdKey.SetValue(null, $"\"{executable}\" open -f \"%1\"");

                Console.WriteLine("Context menu installed.");

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Install failed: " + ex.Message);
                return -1;
            }
        }

        private static int Uninstall()
        {
            if (!IsRunningAsAdmin())
            {
                return RunAsAdmin();
            }

            try
            {
                using var key = Registry.ClassesRoot.OpenSubKey(OpenAdAdminKey);
                if (key != null)
                    Registry.ClassesRoot.DeleteSubKeyTree(OpenAdAdminKey);
                Console.WriteLine("Context menu uninstalled.");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Uninstall failed: " + ex.Message);
                return -1;
            }
        }

        private unsafe static void Open(string filePath)
        {
            uint cchOut = 0;
            var result = PInvoke.AssocQueryString(ASSOCF.ASSOCF_NONE,
                ASSOCSTR.ASSOCSTR_COMMAND,
                Path.GetExtension(filePath),
                null,
                null,
                ref cchOut);
            if (result.Succeeded)
            {
                Span<char> commandBuffer = stackalloc char[(int)cchOut];
                result = PInvoke.AssocQueryString(ASSOCF.ASSOCF_NONE,
                    ASSOCSTR.ASSOCSTR_COMMAND,
                    Path.GetExtension(filePath),
                    null,
                    commandBuffer,
                    ref cchOut);

                var command = commandBuffer.ToString();
                System.Console.WriteLine(command);

                var argv = PInvoke.CommandLineToArgv(command, out var argc);
                var array = new string[argc];
                try
                {
                    for (int i = 0; i < argc; i++)
                    {
                        var arg = argv[i];
                        var span = arg.ToString();
                        span = span.Replace("%1", filePath);
                        array[i] = span;
                    }
                }
                finally
                {
                    PInvoke.LocalFree(new Windows.Win32.Foundation.HLOCAL(argv));
                }

                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = array[0],
                    WorkingDirectory = Path.GetDirectoryName(filePath) ?? string.Empty,
                    UseShellExecute = true,
                    Verb = "runas"
                };
                for (int i = 1; i < array.Length; i++)
                {
                    startInfo.ArgumentList.Add(array[i]);
                }
                System.Diagnostics.Process.Start(startInfo);
            }
        }


        static int Main(string[] args)
        {
            return new RootCommand("Open file with associated program run as administrator.")
            {
                Subcommands =
                {
                    BuildInstallCommand(),
                    BuildUninstallCommand(),
                    BuildOpenCommand(),
                }
            }.Parse(args).Invoke();
        }
    }
}
