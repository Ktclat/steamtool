# 构建与验证

## 推荐环境

- Git（需要子模块支持）；
- .NET SDK 6.0.428；
- Windows 桌面构建建议使用 Windows 10/11 x64。

官方 `2.8.6` 的 GitHub Actions 明确使用 .NET 6.0.x。该版本年代较早，使用更新 SDK 时可能遇到
旧 NuGet 包、目标框架或分析器兼容问题，因此优先使用与上游一致的 SDK。

仓库根目录的 `global.json` 将 SDK 固定为 .NET 6 的最后一个 SDK `6.0.428`，避免 GitHub Runner
或开发机上安装的较新 SDK 被自动选中。

不要用整个解决方案的 `dotnet build` 代替官方流程。解决方案还包含 Desktop Bridge、iOS、
.NET Framework 3.5 和发布工具项目，需要额外 workload、Desktop MSBuild 与旧目标包；上游 CI
是先还原解决方案，再单独构建桌面应用项目。

## 获取完整源码

新克隆时初始化全部子模块：

```powershell
git -c core.longpaths=true clone --recurse-submodules https://github.com/Ktclat/steamtool.git
Set-Location steamtool
```

已有工作区可执行：

```powershell
git submodule sync --recursive
git -c core.longpaths=true submodule update --init --recursive
```

子模块必须位于 `.gitmodules` 锁定的提交。不要将子模块目录改为普通目录后整体提交。
Windows 上 `references/reactive` 含有超过默认路径长度限制的历史测试文件，因此上例为该次检出临时启用
Git 长路径支持；不需要修改全局 Git 配置。

## Windows 桌面端

```powershell
dotnet restore '.\SteamToolsV2+.sln'
dotnet build '.\src\ST.Client.Desktop.Avalonia.App\ST.Client.Avalonia.App.csproj' -c Release
```

## 上游测试集

```powershell
dotnet test '.\tests\Common.UnitTest\Common.UnitTest.csproj' -c Release
dotnet test '.\tests\ST.Client.UnitTest\ST.Client.UnitTest.csproj' -c Release
dotnet test '.\tests\ST.Client.Desktop.UnitTest\ST.Client.Desktop.UnitTest.csproj' -c Release
```

## 本分支验证记录

2026-09-08 在 Windows、.NET SDK 9.0.309 下完成了以下验证：

- `ST.Client` 的 `net6.0` Release 目标构建成功：0 个错误；
- `ST.Client.UnitTest` 测试成功：11/11 通过；
- 官方桌面应用项目编译至最终 Windows App SDK 校验时，因 SDK 9 与该历史项目使用的 Windows SDK
  投影不匹配而报 `NETSDK1148`。这不是广告策略代码的编译错误，应在官方要求的 .NET SDK 6.0.x
  环境中复核完整桌面应用，而不应通过改动项目目标框架规避。

还原期间会报告 2.8.6 固定的部分旧依赖存在已知安全公告，包括 MessagePack 2.4.59、
System.Text.Json 6.0.2、AutoMapper 12.0.0 与 SkiaSharp 2.88.3。本次修改坚持最小范围，未升级依赖；
后续发布前应另开兼容性与安全升级任务，并重新执行完整回归测试。

## GitHub Actions 测试产物

`.github/workflows/dotnet.yml` 在影响应用构建的内容推送到 `main` 时自动运行，也支持在 Actions 页面
手动运行。该工作流在 `windows-2022` 上执行单元测试，并使用 `win-x64` 发布配置生成自包含 Release
便携版，随后上传名称形如 `SteamTool-2.8.6-adfree-win-x64-<commit>` 的临时 Artifact，保留 14 天。

Artifact 内的 `BUILD-INFO.txt` 记录源码提交、SDK、构建方式和主程序 SHA-256。该产物不读取上游私有
凭据、不进行代码签名，也不会自动创建 GitHub Release；仅用于本分支功能验证，Windows 可能显示
未知发布者或 SmartScreen 提示。

## 发布注意事项

`packaging/build.ps1` 是上游发布流水线的一部分，会清理发布目录并依赖特定发布参数、签名材料和机密配置。
在建立自己的签名与发布流程前，不要把“能本地编译”等同于“可以生成可信官方安装包”。

构建输出（例如 `bin/`、`obj/`、安装包和本地凭据）不应提交到仓库。
