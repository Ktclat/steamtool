# 广告策略与实现审阅

## 上游 2.8.6 的广告链路

官方 `2.8.6` 的桌面端广告流程集中在以下位置：

1. `src/ST.Client.Desktop.Avalonia/Application/UI/Views/MainWindow.axaml.cs`
   在启动任务中调用 `AdvertiseService.Current.InitAdvertise()`。
2. `src/ST.Client/Services/Mvvm/AdvertiseService.cs`
   通过 `ICloudServiceClient.Instance.Advertisement.All()` 获取广告，并将结果写入横向、纵向广告集合。
3. `src/ST.Client.Desktop.Avalonia/Application/UI/Views/Controls/UserControl/AdDialog.axaml`
   绑定 `AdvertiseService.IsShowAdvertise` 和对应广告集合来决定展示。
4. `src/ST.Client/Settings/UISettings.cs` 与
   `src/ST.Client.Desktop.Avalonia/Application/UI/Views/Pages/Settings/Settings_UI.axaml`
   提供“关闭程序内所有广告”设置。

上游的 `CheckShow()` 只有在登录用户的 `UserType` 为 `Sponsor` 且设置值为关闭时才隐藏广告；
这就是官方“赞助用户可关闭广告”的限制位置。

## 本分支的最小改动

本分支仅在 `AdvertiseService.cs` 增加集中式编译期开关：

```csharp
internal const bool EnableAdvertisements = false;
```

该开关同时控制初始状态，并在三个入口执行保护：

- `IsShowAdvertise`：对象创建时即为关闭，避免初始化任务执行前出现短暂可见状态；
- `InitAdvertise()`：不启动广告刷新，并固定当前展示状态为关闭；
- `RefrshAdvertise()`：即使未来出现其他调用方，也不请求广告接口；
- `CheckShow()`：任何登录状态、缓存变化或设置变化都不能重新打开广告展示。

这项处理没有删除广告客户端、数据模型、视图、资源、本地化字符串或赞助用户逻辑，
因此差异小、审计简单，也不会误伤与赞助身份有关的非广告功能。

## 恢复上游行为

将 `EnableAdvertisements` 改为 `true` 即可恢复官方 `2.8.6` 的初始化、请求和赞助用户显示判定。
恢复后应重新执行构建与界面验证。

## 验证要点

- 启动后 `IAdvertisementClient.All()` 不应被广告初始化流程调用；
- 横向与纵向 `AdDialog` 均不可见；
- 登录、退出或切换 `UISettings.IsShowAdvertise` 后，广告仍不可见；
- 赞助身份、捐助列表及其他用户中心功能保持可用。

当前已完成 `ST.Client` net6.0 Release 编译和 `ST.Client.UnitTest` 11/11 测试。由于当前机器缺少
上游指定的 .NET SDK 6.0.x，完整桌面 App 在 SDK 9 的 Windows SDK 投影校验处受阻；上述网络请求、
界面可见性和账号功能仍需在可运行桌面 App 的环境中做最终动态检查。
