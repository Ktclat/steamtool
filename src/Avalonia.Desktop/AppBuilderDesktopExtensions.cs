using Avalonia.Controls;

// https://github.com/AvaloniaUI/Avalonia/blob/0.10.13/src/Avalonia.Desktop/AppBuilderDesktopExtensions.cs

namespace Avalonia
{
    public static class AppBuilderDesktopExtensions
    {
        public static TAppBuilder UsePlatformDetect<TAppBuilder>(this TAppBuilder builder)
            where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
        {
            builder.UseWin32();
            builder.UseSkia();
            return builder;
        }
    }
}
