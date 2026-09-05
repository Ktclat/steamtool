using EPlatform = System.Platform;

namespace System.Application.Services;

public interface IDeviceInfoPlatformService
{
    static IDeviceInfoPlatformService? Interface => DI.Get_Nullable<IDeviceInfoPlatformService>();

    string Model { get; }

    string Manufacturer { get; }

    string Name { get; }

    string VersionString { get; }

    DeviceType DeviceType { get; }

    bool IsChromeOS { get; }

    bool IsUWP { get; }

    bool IsWinUI { get; }

    DeviceIdiom Idiom { get; }

    static EPlatform Platform => EPlatform.Windows;
}
