# OpenAsAdmin

OpenAsAdmin is a Windows command-line tool that allows you to open any file with administrator privileges using its associated application. It can also be installed to the right-click context menu for quick access.

## Features

- ✅ Open any file with administrator privileges
- ✅ Support for installing to the right-click context menu
- ✅ Support for uninstalling from the right-click context menu
- ✅ Automatically uses the file's associated application

## Installation

### Installation Steps

1. Download the latest version of the executable from the [Releases](https://github.com/JerryJian/OpenAsAdmin/releases) page
2. Save the downloaded `OpenAsAdmin.exe` file to your preferred location
3. Navigate to the directory where `OpenAsAdmin.exe` is saved
4. Run the installation command:

```bash
OpenAsAdmin.exe install -n "Open as Administrator"
```

## Usage

### Using from the Right-Click Menu

1. Right-click on any file
2. Select "Open as Administrator" (or the name you specified during installation)

### Using from the Command Line

```bash
OpenAsAdmin.exe open -f "path/to/your/file.txt"
```

## Command Line Parameters

### Install Command

```bash
OpenAsAdmin.exe install --name "Menu Name" [-n "Menu Name"]
```

- `--name` or `-n`: The name displayed in the context menu (required)

### Uninstall Command

```bash
OpenAsAdmin.exe uninstall
```

### Open File Command

```bash
OpenAsAdmin.exe open --file "File Path" [-f "File Path"]
```

- `--file` or `-f`: The path of the file to open (required)

## Building

### Building Steps

1. Clone the repository:

```bash
git clone https://github.com/JerryJian/OpenAsAdmin.git
```

2. Navigate to the project directory:

```bash
cd OpenAsAdmin
```

3. Build using .NET CLI:

```bash
dotnet build -c Release
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Contributing

Contributions are welcome! Please submit issues and pull requests.

## Support

If you encounter any problems or have any suggestions, please open an issue on the [Issues](https://github.com/yourusername/OpenAsAdmin/issues) page.
