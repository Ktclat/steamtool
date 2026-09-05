namespace System.Application.Markup
{
    public sealed class OnPlatformExtension : MarkupExtension<bool>
    {
        public OnPlatformExtension(string member) : base(member)
        {

        }

        public override bool ProvideValue(IServiceProvider serviceProvider) =>
            member switch
            {
                "Windows" => true,
                "Windows7" => OperatingSystem2.IsWindows7(),
                "Windows10AtLeast" => OperatingSystem2.IsWindows10AtLeast(),
                "Windows11AtLeast" => OperatingSystem2.IsWindows11AtLeast(),
                "Mono" => OperatingSystem2.IsRunningOnMono(),
                "Windows10PackageIdentity" => OperatingSystem2.IsRunningAsUwp(),
                "DesktopBridge" => DesktopBridge.IsRunningAsUwp,
                _ => false,
            };
    }
}
