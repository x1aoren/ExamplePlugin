# Hello Plugin - 示例插件

这是一个完整的示例插件项目，演示了如何为 ObsMCLauncher 开发插件。

## 📋 功能展示

- ✅ 实现 `ILauncherPlugin` 接口
- ✅ 插件加载时显示通知
- ✅ 订阅启动器事件
- ✅ 使用插件数据目录
- ✅ 正确的插件元数据配置

## 🛠️ 构建步骤

### 1. 先构建启动器

```bash
cd H:\projects\ObsMCLauncher
dotnet build
```

### 2. 构建插件

```bash
cd ExamplePlugin
dotnet build
```

### 3. 打包插件

将以下文件打包成 ZIP：
- `bin/Debug/net8.0-windows/ExamplePlugin.dll`
- `bin/Debug/net8.0-windows/plugin.json`
- `icon.png` (可选)

文件结构应该是：
```
HelloPlugin.zip
├── ExamplePlugin.dll (重命名为 example-hello-plugin.dll)
├── plugin.json
└── icon.png (可选)
```

### 4. 安装插件

将 ZIP 文件解压到启动器安装目录的 `plugins` 文件夹：

```
启动器目录/plugins/example-hello-plugin/
```

例如：
```
C:\Program Files\ObsMCLauncher\plugins\example-hello-plugin\
```

或开发环境：
```
H:\projects\ObsMCLauncher\bin\Debug\net8.0-windows\plugins\example-hello-plugin\
```

最终目录结构：
```
ObsMCLauncher/
├── ObsMCLauncher.exe
└── plugins/
    └── example-hello-plugin/
        ├── example-hello-plugin.dll
        ├── plugin.json
        └── icon.png (可选)
```

### 5. 启动启动器

重启 ObsMCLauncher，插件会自动加载并显示欢迎通知。

## 📝 代码说明

### HelloPlugin.cs

```csharp
public class HelloPlugin : ILauncherPlugin
{
    // 必须实现的属性
    public string Id => "example-hello-plugin";
    public string Name => "Hello Plugin";
    public string Version => "1.0.0";
    public string Author => "ObsMCLauncher Team";
    public string Description => "示例插件";
    
    // 插件加载时调用
    public void OnLoad(IPluginContext context)
    {
        // 显示通知
        context.NotificationManager.ShowNotification(
            "Hello Plugin",
            "示例插件已成功加载！",
            NotificationType.Success,
            3
        );
        
        // 订阅事件
        context.SubscribeEvent("GameLaunched", OnGameLaunched);
    }
    
    // 插件卸载时调用
    public void OnUnload() { }
    
    // 启动器关闭时调用
    public void OnShutdown() { }
}
```

### plugin.json

```json
{
  "id": "example-hello-plugin",
  "name": "Hello Plugin",
  "version": "1.0.0",
  "author": "ObsMCLauncher Team",
  "description": "一个简单的示例插件",
  "minLauncherVersion": "1.0.0",
  "permissions": ["notification"]
}
```

## 🎨 自定义UI页面（可选）

如果你想为插件添加自己的UI页面，可以创建一个 WPF Page 或 UserControl：

```csharp
public void OnLoad(IPluginContext context)
{
    // 创建自定义页面
    var myPage = new MyPluginPage();
    
    // 注册到"更多"页面的导航栏
    context.RegisterTab("我的插件", myPage, "Star");
}
```

## 🔗 更多信息

查看完整的插件开发文档：
[Plugin-Development.md](https://github.com/mcobs/ObsMCLauncher/blob/main/Plugin-Development.md)

