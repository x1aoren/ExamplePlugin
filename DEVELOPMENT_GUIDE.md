# Avalonia插件开发指南

本文档详细说明如何基于此示例插件开发自己的ObsMCLauncher插件。

## 项目结构

```
YourPlugin/
├── YourPlugin.csproj          # 项目文件
├── Plugin.cs                  # 插件主类（实现 ILauncherPlugin）
├── plugin.json                # 插件元数据（必需）
├── README.md                  # 插件说明（必需）
├── icon.png                   # 插件图标（可选，128x128）
└── LICENSE                    # 开源协议（推荐）
```

## 1. 创建新插件项目

### 方法一：复制此示例项目

1. 复制整个 `ExamplePlugin` 文件夹，重命名为你的插件名称
2. 修改以下文件：
   - `YourPlugin.csproj`：修改项目名称
   - `Plugin.cs`：修改命名空间和类名
   - `plugin.json`：修改所有字段
   - `README.md`：更新文档

### 方法二：从头创建

```bash
# 创建新项目
dotnet new classlib -n YourPlugin -f net8.0

cd YourPlugin

# 添加项目引用
dotnet add reference ..\ObsMCLauncher\ObsMCLauncher.Core\ObsMCLauncher.Core.csproj
```

## 2. 项目文件配置

修改 `YourPlugin.csproj`：

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <OutputType>Library</OutputType>
  </PropertyGroup>

  <ItemGroup>
    <!-- 引用ObsMCLauncher.Core项目 -->
    <ProjectReference Include="..\ObsMCLauncher\ObsMCLauncher.Core\ObsMCLauncher.Core.csproj" />
  </ItemGroup>

  <ItemGroup>
    <!-- 复制必要文件到输出目录 -->
    <None Update="plugin.json">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
    <None Update="README.md">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
    <None Update="icon.png" Condition="Exists('icon.png')">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
  </ItemGroup>

</Project>
```

## 3. 实现插件接口

创建 `Plugin.cs` 文件：

```csharp
using System;
using System.Diagnostics;
using ObsMCLauncher.Core.Plugins;

namespace YourPlugin
{
    public class Plugin : ILauncherPlugin
    {
        private IPluginContext? _context;

        // 必需属性
        public string Id => "your-plugin-id";
        public string Name => "Your Plugin Name";
        public string Version => "1.0.0";
        public string Author => "Your Name";
        public string Description => "Your plugin description";

        public void OnLoad(IPluginContext context)
        {
            _context = context;

            Debug.WriteLine($"[{Name}] 插件加载成功");

            // 显示欢迎通知
            context.ShowNotification(
                Name,
                "插件已成功加载",
                "info",
                3
            );

            // 注册标签页（显示在"更多"页面）
            context.RegisterTab(
                "插件名称",          // 标签页标题
                "your-plugin-tab",  // 标签页ID（唯一）
                "Star"              // 图标名称（可选）
            );

            // 注册主页卡片
            context.RegisterHomeCard(
                "your-card-id",     // 卡片ID（唯一）
                "卡片标题",         // 卡片标题
                "卡片描述",         // 卡片描述
                "🌟",               // 图标（emoji或文本）
                "navigate:more"     // 点击命令（可选）
            );

            // 订阅事件
            context.SubscribeEvent("GameLaunched", OnGameLaunched);
            context.SubscribeEvent("GameClosed", OnGameClosed);
        }

        public void OnUnload()
        {
            // 卸载时清理资源
            _context?.UnregisterHomeCard("your-card-id");
            Debug.WriteLine($"[{Name}] 插件卸载");
        }

        public void OnShutdown()
        {
            // 启动器关闭时保存数据
            Debug.WriteLine($"[{Name}] 启动器关闭");
        }

        private void OnGameLaunched(object? eventData)
        {
            Debug.WriteLine($"[{Name}] 游戏启动: {eventData}");
        }

        private void OnGameClosed(object? eventData)
        {
            Debug.WriteLine($"[{Name}] 游戏关闭: {eventData}");
        }
    }
}
```

## 4. 创建插件元数据

创建 `plugin.json` 文件：

```json
{
  "id": "your-plugin-id",
  "name": "Your Plugin Name",
  "version": "1.0.0",
  "author": "Your Name",
  "description": "Your plugin description",
  "repository": "https://github.com/yourusername/your-plugin",
  "minLauncherVersion": "1.0.0",
  "dependencies": [],
  "tags": ["功能标签1", "功能标签2", "Windows", "Linux", "macOS"],
  "category": "utility",
  "homepage": "https://your-plugin-website.com",
  "license": "MIT"
}
```

## 5. 创建README.md

创建 `README.md` 文件，至少包含：
- 插件名称和描述
- 功能列表
- 安装说明
- 使用说明
- 构建步骤

## 6. API参考

### 事件系统

```csharp
// 订阅事件
context.SubscribeEvent("GameLaunched", OnGameLaunched);
context.SubscribeEvent("GameClosed", OnGameClosed);

// 发布自定义事件
context.PublishEvent("MyCustomEvent", eventData);
```

### 通知系统

```csharp
// 显示通知
var notificationId = context.ShowNotification(
    "标题",
    "消息内容",
    "info",      // 类型：info, success, warning, error, progress
    3            // 持续时间（秒），null表示默认3秒，0表示无限
);

// 更新进度通知
context.UpdateNotification(notificationId, "下载中 50%", 50);

// 关闭通知
context.CloseNotification(notificationId);
```

### 主页卡片

```csharp
// 注册卡片
context.RegisterHomeCard(
    "card-id",
    "标题",
    "描述",
    "🌟",                    // 图标
    "url:https://example.com" // 命令
);

// 支持的命令格式：
// "url:https://example.com" - 打开外部链接
// "navigate:more" - 跳转到启动器内部页面
// null - 卡片不可点击

// 注销卡片
context.UnregisterHomeCard("card-id");
```

### 插件数据目录

```csharp
string dataDir = context.PluginDataDirectory;

// 保存配置
var configPath = Path.Combine(dataDir, "config.json");
File.WriteAllText(configPath, "{}");

// 创建数据目录
var dataFolder = Path.Combine(dataDir, "data");
Directory.CreateDirectory(dataFolder);
```

## 7. 构建和测试

### 构建插件

```bash
dotnet build -c Release
```

### 安装插件

1. 创建插件目录：
   ```
   启动器目录\OMCL\plugins\your-plugin-id\
   ```

2. 复制以下文件：
   - `YourPlugin.dll` → `your-plugin-id.dll`
   - `plugin.json`
   - `README.md`
   - `icon.png`（可选）

### 调试插件

使用 Visual Studio 附加到 `ObsMCLauncher.Desktop` 进程进行调试。

## 8. 发布插件

### 准备发布包

```bash
dotnet build -c Release

cd bin/Release/net8.0/

# 重命名DLL文件
copy YourPlugin.dll your-plugin-id.dll

# 创建ZIP包
Compress-Archive -Path your-plugin-id.dll,plugin.json,README.md,icon.png -DestinationPath YourPlugin-v1.0.0.zip
```

### 发布到GitHub

1. 创建新 Release
2. Tag: `v1.0.0`
3. 上传 ZIP 文件

### 提交到插件市场

在 [ObsMCLauncher-PluginMarket](https://github.com/mcobs/ObsMCLauncher-PluginMarket) 提交 PR 或 Issue。

## 9. 最佳实践

### 错误处理

```csharp
public void OnLoad(IPluginContext context)
{
    try
    {
        // 插件初始化代码
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"[{Name}] 加载失败: {ex.Message}");
        context.ShowNotification(
            Name,
            $"插件加载失败: {ex.Message}",
            "error",
            5
        );
    }
}
```

### 资源清理

```csharp
public void OnUnload()
{
    // 清理所有注册的资源
    _context?.UnregisterHomeCard("card1");
    _context?.UnregisterHomeCard("card2");

    // 取消所有事件订阅
    // 注意：插件系统会自动清理事件订阅，但显式清理是好习惯
}
```

### 跨平台兼容性

```csharp
// 使用 Path.Combine 处理文件路径
var configPath = Path.Combine(context.PluginDataDirectory, "config.json");

// 避免使用平台特定的API
// 使用 .NET Standard 2.0/2.1 兼容的API
```

## 10. 常见问题

### Q: 插件可以访问哪些启动器功能？
A: 插件通过 `IPluginContext` 可以访问：
- 事件订阅和发布
- UI扩展（标签页、主页卡片）
- 通知系统
- 插件数据目录
- 启动器版本信息

### Q: 插件如何保存数据？
A: 使用 `context.PluginDataDirectory` 获取专属数据目录，在该目录下保存配置和数据文件。

### Q: 插件可以添加新的UI页面吗？
A: 可以，使用 `context.RegisterTab()` 在"更多"页面注册标签页。启动器会根据 `tabId` 显示对应的UI内容。

### Q: 插件出错会导致启动器崩溃吗？
A: 不会，启动器会捕获插件异常并隔离错误，只会禁用有问题的插件。

### Q: 如何调试插件？
A: 使用 Visual Studio 附加到 `ObsMCLauncher.Desktop` 进程进行调试。

## 11. 示例代码

完整的示例代码请参考此 `ExamplePlugin` 项目。

## 12. 更多资源

- [ObsMCLauncher 官方仓库](https://github.com/mcobs/ObsMCLauncher)
- [插件开发文档](https://github.com/mcobs/ObsMCLauncher/blob/main/Plugin-Development.md)
- [.NET 8.0 文档](https://learn.microsoft.com/zh-cn/dotnet/)
- [Avalonia UI 文档](https://docs.avaloniaui.net/)

---

**祝您开发愉快！**