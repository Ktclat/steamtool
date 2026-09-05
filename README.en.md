# Steam++ 2.8.6 Windows x64 Edition

This repository is derived from [BeyondDimension/SteamTools](https://github.com/BeyondDimension/SteamTools), based on tag [`2.8.6`](https://github.com/BeyondDimension/SteamTools/releases/tag/2.8.6) and commit [`6a5f8db`](https://github.com/BeyondDimension/SteamTools/commit/6a5f8db69f92a98a0503234f3b34433e2924c1c1).

This branch is maintained as a Windows 64-bit learning edition of Steam++ 2.8.6:

- Windows x64 is the only supported build and publish target;
- Android, iOS, macOS, Linux, Windows x86, and ARM build content has been removed;
- in-app advertisement services, APIs, models, views, settings, and placeholder assets have been removed;
- original core features and sponsor/donation status features remain because they are not part of the ad-delivery path.

> `source/` is an upstream SteamTools V1 archive. It is excluded from the 2.8.6 solution, build, and release workflows and is retained unchanged to avoid deleting historical source without separate approval.

## Build

.NET 6 SDK, Git with submodule support, and Windows 10 SDK 10.0.19041 are required. Initialize dependencies before the first build:

```powershell
git submodule update --init --recursive
dotnet restore src/ST.Client.Desktop.Avalonia.App/ST.Client.Avalonia.App.csproj --runtime win7-x64 -p:Platform=x64 -p:Configuration=Release
```

Publish the self-contained Windows x64 build:

```powershell
dotnet publish src/ST.Client.Desktop.Avalonia.App/ST.Client.Avalonia.App.csproj --configuration Release --runtime win7-x64 --no-restore -p:Platform=x64 -p:PublishProfile=win-x64
```

Publish the framework-dependent Windows x64 build:

```powershell
dotnet publish src/ST.Client.Desktop.Avalonia.App/ST.Client.Avalonia.App.csproj --configuration Release --runtime win7-x64 --no-restore -p:Platform=x64 -p:PublishProfile=fd-win-x64
```

Alternatively, run `packaging/build.ps1` to create both variants. Output is written below `src/ST.Client.Desktop.Avalonia.App/bin/Release/Publish`.

## License

This project remains licensed under the original [GNU General Public License v3.0](LICENSE). Distributions of modified builds must preserve the copyright and license notices and provide the corresponding source as required by GPLv3.
