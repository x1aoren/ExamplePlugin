# Hello Plugin - Avalonia示例插件

这是一个完整的示例插件项目，演示了如何为 ObsMCLauncher (Avalonia版本) 开发插件。

## 📋 功能展示

- ✅ 实现 `ILauncherPlugin` 接口 (Avalonia版本)
- ✅ 插件加载时显示通知
- ✅ 订阅启动器事件 (GameLaunched, GameClosed)
- ✅ 注册插件标签页到"更多"页面
- ✅ 注册示例主页卡片 (新增功能)
- ✅ 使用插件数据目录
- ✅ 正确的插件元数据配置

## 🛠️ 构建步骤

### 1. 先构建启动器

```bash
cd H:\projects\Project ObsMCLauncher\ObsMCLauncher
dotnet build
```

### 2. 构建插件

```bash
cd H:\projects\Project ObsMCLauncher\ExamplePlugin
dotnet build
```

### 3. 测试插件

将以下文件复制到启动器的插件目录：

```
启动器运行目录\OMCL\plugins\example-hello-plugin\
```

例如开发环境：
```
H:\projects\Project ObsMCLauncher\ObsMCLauncher\bin\Debug\net8.0\OMCL\plugins\example-hello-plugin\
```

需要复制的文件：
- `bin/Debug/net8.0/ExamplePlugin.dll` (重命名为 example-hello-plugin.dll)
- `plugin.json`
- `README.md`
- `icon.png` (可选)

最终目录结构：
```
ObsMCLauncher/
├── ObsMCLauncher.Desktop.dll
└── OMCL/
    └── plugins/
        └── example-hello-plugin/
            ├── example-hello-plugin.dll
            ├── plugin.json
            ├── README.md
            └── icon.png (可选)
```

### 4. 启动启动器

重启 ObsMCLauncher，插件会自动加载并显示欢迎通知。

## 📝 代码说明

### HelloPlugin.cs - Avalonia版本

```csharp
public class HelloPlugin : ILauncherPlugin
{
    private IPluginContext? _context;

    public string Id => "example-hello-plugin";
    public string Name => "Hello Plugin";
    public string Version => "1.0.0";
    public string Author => "ObsMCLauncher Team";
    public string Description => "一个简单的示例插件，演示插件开发";

    public void OnLoad(IPluginContext context)
    {
        _context = context;

        // 显示欢迎通知
        context.ShowNotification(
            "Hello Plugin",
            "Hello World! 插件已成功加载。",
            "info",
            3
        );

        // 注册插件标签页（Avalonia版本使用tabId而不是UI控件）
        context.RegisterTab("Hello", "example-hello-tab", "Heart");

        // 注册示例主页卡片
        context.RegisterHomeCard(
            "example-hello-card",
            "Hello Plugin 示例",
            "这是一个示例插件的主页卡片，点击可以查看插件详情。",
            "🌟",
            "navigate:more"
        );

        // 注册第二个示例卡片（打开外部链接）
        context.RegisterHomeCard(
            "example-wiki-card",
            "Minecraft Wiki",
            "访问中文Minecraft Wiki获取游戏信息",
            "📖",
            "url:https://zh.minecraft.wiki"
        );

        // 订阅启动器事件
        context.SubscribeEvent("GameLaunched", OnGameLaunched);
        context.SubscribeEvent("GameClosed", OnGameClosed);
    }

    public void OnUnload()
    {
        // 卸载时移除注册的主页卡片
        _context?.UnregisterHomeCard("example-hello-card");
        _context?.UnregisterHomeCard("example-wiki-card");
    }

    public void OnShutdown()
    {
        // 启动器关闭时保存数据
    }

    private void OnGameLaunched(object? eventData)
    {
        // 游戏启动时显示通知
        _context?.ShowNotification(
            "游戏启动",
            "Minecraft游戏已启动！",
            "success",
            2
        );
    }

    private void OnGameClosed(object? eventData)
    {
        // 游戏关闭时显示通知
        _context?.ShowNotification(
            "游戏关闭",
            "Minecraft游戏已关闭。",
            "info",
            2
        );
    }
}
```

### plugin.json - 完整格式

```json
{
  "id": "example-hello-plugin",
  "name": "Hello Plugin",
  "version": "1.0.0",
  "author": "ObsMCLauncher Team",
  "description": "一个简单的示例插件，演示如何为 ObsMCLauncher 开发插件。",
  "repository": "https://github.com/mcobs/ObsMCLauncher",
  "minLauncherVersion": "1.0.0",
  "dependencies": [],
  "tags": ["示例", "教程", "Windows", "Linux", "macOS"],
  "category": "utility",
  "homepage": "https://github.com/mcobs/ObsMCLauncher",
  "license": "MIT"
}
```

## 🎨 新增功能：主页卡片

Avalonia版本的插件系统新增了主页卡片功能，插件可以在启动器主页显示自定义卡片：

```csharp
// 注册主页卡片
context.RegisterHomeCard(
    "card-id",           // 卡片唯一标识符
    "卡片标题",          // 卡片标题
    "卡片描述",          // 卡片描述
    "🌟",                // 图标（emoji或文本）
    "navigate:more"      // 点击命令（跳转到"更多"页面）
);

// 支持的命令格式：
// "url:https://example.com" - 打开外部链接
// "navigate:more" - 跳转到启动器内部页面
// null - 卡片不可点击（仅展示信息）
```

## 🔗 更多信息

查看完整的插件开发文档：
[Plugin-Development.md](https://github.com/mcobs/ObsMCLauncher/blob/main/Plugin-Development.md)

## 📚 迁移说明

此插件已从WPF版本迁移到Avalonia版本，主要变化：

1. **项目配置**：从`net8.0-windows`改为`net8.0`，移除WPF依赖
2. **接口引用**：使用`ObsMCLauncher.Core.Plugins`命名空间
3. **UI注册**：不再直接传递UI控件，使用`tabId`标识标签页
4. **新增功能**：支持主页卡片注册
5. **通知系统**：使用新的`ShowNotification` API

