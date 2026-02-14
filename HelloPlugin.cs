using System;
using System.Diagnostics;
using ObsMCLauncher.Core.Plugins;

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

            // 订阅启动器事件（示例）
            context.SubscribeEvent("GameLaunched", OnGameLaunched);
            context.SubscribeEvent("GameClosed", OnGameClosed);
        }

        public void OnUnload()
        {
            // 卸载时移除注册的主页卡片
            _context?.UnregisterHomeCard("example-hello-card");
            _context?.UnregisterHomeCard("example-wiki-card");

            Debug.WriteLine($"[HelloPlugin] 插件卸载");
        }

        public void OnShutdown()
        {
            Debug.WriteLine($"[HelloPlugin] 启动器即将关闭，保存数据");

            // 可以在这里保存插件配置等
            // var configPath = System.IO.Path.Combine(_context.PluginDataDirectory, "config.json");
            // System.IO.File.WriteAllText(configPath, "{}");
        }

        private void OnGameLaunched(object? eventData)
        {
            Debug.WriteLine($"[HelloPlugin] 检测到游戏启动事件: {eventData}");

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
            Debug.WriteLine($"[HelloPlugin] 检测到游戏关闭事件: {eventData}");

            // 游戏关闭时显示通知
            _context?.ShowNotification(
                "游戏关闭",
                "Minecraft游戏已关闭。",
                "info",
                2
            );
        }
    }
}

