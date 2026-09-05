# 源码结构（Windows x64）

当前源码树只维护 Steam++ 2.8.6 的 Windows x64 客户端。

- `ST.Client.Desktop.Avalonia.App`：Windows x64 主程序与发布入口。
- `ST.Client.Desktop.Avalonia`：Avalonia 桌面界面。
- `ST.Client.Desktop.Windows`：Windows 平台服务实现。
- `ST.Client.Windows.ResSecrets`：Windows 平台资源密钥实现。
- `ST.Client.Windows.ScheduledTasks`：Windows 计划任务支持。
- `ST.Client.ReverseProxy.Yarp`：Windows 网络加速实现。
- `ST.Client`、`ST`、`Common.*`：共享业务与基础库；它们现在只由 Windows 构建图使用。
- `ST.Services.CloudService*`：云服务客户端与模型；广告接口与模型已删除。
- `ST.Client.Desktop.Avalonia.App.Bridge*`：可选的 Windows MSIX/Desktop Bridge 打包项目。
- `ST.Tools.*`：仍被保留的 Windows 或通用开发辅助工具。

Android、iOS、macOS、Linux、Xamarin、MAUI、X11、Avalonia Native 和旧多平台发布工具目录已移除。解决方案只提供 `Debug|x64` 与 `Release|x64` 两个配置。
