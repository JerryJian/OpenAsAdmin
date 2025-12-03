# OpenAsAdmin

OpenAsAdmin 是一个 Windows 命令行工具，允许您以管理员权限打开任何文件，通过关联的应用程序运行。它还可以安装到右键菜单，方便快速使用。

## 功能特点

- ✅ 以管理员权限打开任意文件
- ✅ 支持安装到右键菜单
- ✅ 支持从右键菜单卸载
- ✅ 命令行界面简洁易用
- ✅ 自动使用文件关联的应用程序

## 安装

### 安装步骤

1. 从 [Releases](https://github.com/JerryJian/OpenAsAdmin/releases) 页面下载最新版本的可执行文件
2. 将下载的 `OpenAsAdmin.exe` 文件保存到您喜欢的位置
3. 以管理员权限运行命令提示符
4. 导航到保存 `OpenAsAdmin.exe` 的目录
5. 运行安装命令：

```bash
OpenAsAdmin.exe install -n "以管理员身份打开"
```

## 使用方法

### 从右键菜单使用

1. 右键单击任何文件
2. 选择 "以管理员身份打开"（或您在安装时指定的名称）

### 从命令行使用

```bash
OpenAsAdmin.exe open -f "path/to/your/file.txt"
```

## 命令行参数

### 安装命令

```bash
OpenAsAdmin.exe install --name "菜单名称" [-n "菜单名称"]
```

- `--name` 或 `-n`: 右键菜单中显示的名称（必填）

### 卸载命令

```bash
OpenAsAdmin.exe uninstall
```

### 打开文件命令

```bash
OpenAsAdmin.exe open --file "文件路径" [-f "文件路径"]
```

- `--file` 或 `-f`: 要打开的文件路径（必填）

## 构建

### 构建步骤

1. 克隆仓库：

```bash
git clone https://github.com/JerryJian/OpenAsAdmin.git
```

2. 进入项目目录：

```bash
cd OpenAsAdmin
```

3. 使用 .NET CLI 构建：

```bash
dotnet build -c Release
```

4. 构建输出将在 `OpenAsAdmin/bin/Release/net6.0/` 目录中

## 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情

## 贡献

欢迎提交问题和拉取请求！

## 支持

如果您遇到任何问题或有任何建议，请在 [Issues](https://github.com/yourusername/OpenAsAdmin/issues) 页面提出。