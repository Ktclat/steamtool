# Steam++ 2.8.6 Windows x64 精简版

本仓库派生自 [BeyondDimension/SteamTools](https://github.com/BeyondDimension/SteamTools)，基线为标签 [`2.8.6`](https://github.com/BeyondDimension/SteamTools/releases/tag/2.8.6) 与提交 [`6a5f8db`](https://github.com/BeyondDimension/SteamTools/commit/6a5f8db69f92a98a0503234f3b34433e2924c1c1)。

此分支专用于学习和维护 2.8.6 的 Windows 64 位版本：

- 仅保留 Windows x64 构建、发布配置与运行时代码；
- 删除 Android、iOS、macOS、Linux、Windows x86 和 ARM 发布内容；
- 删除应用内广告服务、广告接口、广告模型、广告视图、广告设置和占位资源；
- 保留 Steam++ 原有核心功能以及赞助/捐赠状态功能；它们不属于广告投放链路。

> `source/` 是上游仓库附带的 SteamTools V1 历史档案，不属于 2.8.6 解决方案、构建或发布流程。为避免未经单独确认删除历史源码，该目录原样保留。

## 构建

需要 .NET 6 SDK、Git（含子模块）和 Windows 10 SDK 10.0.19041。首次构建先初始化依赖：

```powershell
git submodule update --init --recursive
dotnet restore src/ST.Client.Desktop.Avalonia.App/ST.Client.Avalonia.App.csproj --runtime win7-x64 -p:Platform=x64 -p:Configuration=Release
```

生成自包含 Windows x64 版本：

```powershell
dotnet publish src/ST.Client.Desktop.Avalonia.App/ST.Client.Avalonia.App.csproj --configuration Release --runtime win7-x64 --no-restore -p:Platform=x64 -p:PublishProfile=win-x64
```

生成依赖 .NET 6 Desktop Runtime 的 Windows x64 版本：

```powershell
dotnet publish src/ST.Client.Desktop.Avalonia.App/ST.Client.Avalonia.App.csproj --configuration Release --runtime win7-x64 --no-restore -p:Platform=x64 -p:PublishProfile=fd-win-x64
```

也可运行 `packaging/build.ps1` 一次生成以上两个版本。输出目录为 `src/ST.Client.Desktop.Avalonia.App/bin/Release/Publish`。

## 测试

```powershell
dotnet test tests/Common.UnitTest/Common.UnitTest.csproj --configuration Release --runtime win7-x64 -p:Platform=x64
dotnet test tests/ST.Client.UnitTest/ST.Client.UnitTest.csproj --configuration Release --runtime win7-x64 -p:Platform=x64
```

## 许可证

本项目继续遵循原项目的 [GNU General Public License v3.0](LICENSE)。分发修改版本或二进制文件时，请保留版权与许可证声明，并按 GPLv3 提供相应源代码。
