using System;
using System.Diagnostics;
using ObsMCLauncher.Plugins;
using ObsMCLauncher.Utils;

namespace ExamplePlugin
{
    /// <summary>
    /// Hello Plugin - 示例插件
    /// 演示如何为 ObsMCLauncher 开发插件
    /// </summary>
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
            
            Debug.WriteLine($"[HelloPlugin] 插件加载成功！");
            Debug.WriteLine($"[HelloPlugin] 启动器版本: {context.LauncherVersion}");
            Debug.WriteLine($"[HelloPlugin] 插件数据目录: {context.PluginDataDirectory}");
            
            // 初次加载时显示 hello world 提示
            context.NotificationManager.ShowNotification(
                "Hello Plugin",
                "Hello World",
                NotificationType.Info,
                3
            );
            
            // 创建并注册插件页面
            var pluginPage = new HelloPluginPage();
            context.RegisterTab("Hello", pluginPage, "Heart");
            
            // 订阅启动器事件（示例）
            context.SubscribeEvent("GameLaunched", OnGameLaunched);
        }
        
        public void OnUnload()
        {
            Debug.WriteLine($"[HelloPlugin] 插件卸载");
        }
        
        public void OnShutdown()
        {
            Debug.WriteLine($"[HelloPlugin] 启动器即将关闭，保存数据");
            
            // 可以在这里保存插件配置等
            // var configPath = System.IO.Path.Combine(_context.PluginDataDirectory, "config.json");
            // System.IO.File.WriteAllText(configPath, "{}");
        }
        
        private void OnGameLaunched(object eventData)
        {
            Debug.WriteLine($"[HelloPlugin] 检测到游戏启动事件: {eventData}");
        }
    }
}

