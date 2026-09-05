using Microsoft.Extensions.DependencyInjection;
using System.Application.Services;
using System.Application.Services.Implementation;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Properties;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace System.Application.UI
{
    static class AboutAppInfoPopup
    {
        static readonly IPlatformService platformService = IPlatformService.Instance;
        static int show_runtime_info_counter;
        static DateTime show_runtime_info_last_click_time;
        const int show_runtime_info_counter_max = 5;
        const double show_runtime_info_click_effective_interval = 1.5;
        const string os_ver =
            "[os.ver] ";

        public static string GetInfoString()
        {
            StringBuilder b = new(os_ver);
            var productName = platformService.WindowsProductName;
            var major = Environment.OSVersion.Version.Major;
            var minor = Environment.OSVersion.Version.Minor;
            var build = Environment.OSVersion.Version.Build;
            var revision = platformService.WindowsVersionRevision;
            b.AppendFormat("{0} {1}.{2}.{3}.{4}", productName, major, minor, build, revision);
            var servicePack = Environment.OSVersion.ServicePack;
            if (!string.IsNullOrEmpty(servicePack))
            {
                b.Append(' ');
                b.Append(servicePack);
            }
            var releaseId = platformService.WindowsReleaseIdOrDisplayVersion;
            if (!string.IsNullOrWhiteSpace(releaseId))
            {
                b.Append(" (");
                b.Append(releaseId);
                b.Append(')');
            }
            b.AppendLine();

            b.Append("[os.name] ");
            b.Append(DeviceInfo2.OSNameValue());
            b.AppendLine();

            b.Append("[app.ver] ");
            b.Append(ThisAssembly.DynamicVersion);
            b.AppendLine();

            b.Append("[app.flavor] ");
            if (DesktopBridge.IsRunningAsUwp)
            {
                b.Append("ms-store");
            }
            b.AppendLine();

            b.Append("[app.updcha] ");
            b.Append(ApplicationUpdateServiceBaseImpl.UpdateChannelType);
            b.AppendLine();

            b.Append("[app.install] ");
            b.Append(platformService.IsInstall.ToLowerString());
            b.AppendLine();

            if (OperatingSystem2.IsWindows10AtLeast())
            {
                try
                {
#pragma warning disable CA1416 // 验证平台兼容性
                    var currentPackage = global::Windows.ApplicationModel.Package.Current;
                    var familyName = currentPackage.Id.FamilyName;
                    b.Append("[app.pkg] ");
                    b.Append(familyName);
                    b.AppendLine();
#pragma warning restore CA1416 // 验证平台兼容性
                }
                catch
                {
                }
            }

            b.Append("[memory.usage] ");
            b.Append(IOPath.GetDisplayFileSizeString(Environment.WorkingSet));
            b.AppendLine();

            b.Append("[deploy.mode] ");
            b.Append(IApplication.Instance.DeploymentMode);
            b.AppendLine();

            b.Append("[arch.os] ");
            b.Append(RuntimeInformation.OSArchitecture);
            b.AppendLine();

            b.Append("[arch.proc] ");
            b.Append(RuntimeInformation.ProcessArchitecture);
            b.AppendLine();

            b.Append("[time.start] ");
            GetStartTime(b);
            static void GetStartTime(StringBuilder b)
            {
                string startTimeStr;
                try
                {
                    const string f = "yy-MM-dd HH:mm:ss";
                    const string f2 = "HH:mm:ss";
                    const string f3 = "dd HH:mm:ss";
                    var starttime = ArchiSteamFarm.Core.OS.ProcessStartTime;
                    starttime = starttime.ToLocalTime();
                    var utc_time = starttime.ToUniversalTime();
                    var local = TimeZoneInfo.Local;
                    startTimeStr = utc_time.Hour == starttime.Hour
                        ? starttime.ToString(starttime.Year >= 2100 ? DateTimeFormat.Standard : f)
                        : utc_time.Day == starttime.Day
                        ? $"{utc_time.ToString(f)}({starttime.ToString(f2)} {local.StandardName})"
                        : $"{utc_time.ToString(f)}({starttime.ToString(f3)} {local.StandardName})";
                }
                catch (Exception)
                {
                    startTimeStr = string.Empty;
                }
                b.Append(startTimeStr);
            }
            b.AppendLine();

            b.Append("[device.name] ");
            b.Append(DeviceInfo2.Name());
            b.AppendLine();
            b.Append("[device.model] ");
            b.Append(
                DeviceInfo2.Model()
                );
            b.AppendLine();
            b.Append("[device.ver] ");
            b.Append(DeviceInfo2.VersionString());
            b.AppendLine();
            b.Append("[device.idiom] ");
            b.Append(DeviceInfo2.Idiom());
            b.AppendLine();
            b.Append("[device.type] ");
            b.Append(DeviceInfo2.DeviceType());
            b.AppendLine();
            b.Append("[device.manufacturer] ");
            b.Append(
                DeviceInfo2.Manufacturer()
                );
            b.AppendLine();

            b.Append("[avalonia.ver] ");
            b.Append(GetAssemblyVersion(typeof(Avalonia.Application).Assembly));
            b.AppendLine();

            var controllerType = Type.GetType("Microsoft.AspNetCore.Mvc.ControllerBase, Microsoft.AspNetCore.Mvc.Core");
            if (controllerType != null)
            {
                b.Append("[mvc.ver] ");
                b.Append(GetAssemblyVersion(controllerType.Assembly));
                b.AppendLine();
            }

            b.Append("[di.ver] ");
            b.Append(GetAssemblyVersion(typeof(ServiceCollection).Assembly));
            b.AppendLine();

            b.Append("[skia.ver] ");
            b.Append(GetAssemblyVersion(typeof(SkiaSharp.SKColor).Assembly));
            b.AppendLine();

            b.Append("[harfbuzz.ver] ");
            b.Append(GetAssemblyVersion(typeof(HarfBuzzSharp.NativeObject).Assembly));
            b.AppendLine();

            b.Append("[essentials.supported] ");
            b.Append(Essentials.IsSupported.ToLowerString());
            b.AppendLine();

            var b_str = b.ToString();

            return b_str;
        }

        public static void OnClick()
        {
            var now = DateTime.Now;
            if (show_runtime_info_last_click_time == default || (now - show_runtime_info_last_click_time).TotalSeconds <= show_runtime_info_click_effective_interval)
            {
                show_runtime_info_counter++;
            }
            else
            {
                show_runtime_info_counter = 1;
            }
            show_runtime_info_last_click_time = now;
            if (show_runtime_info_counter >= show_runtime_info_counter_max)
            {
                show_runtime_info_counter = 0;
                show_runtime_info_last_click_time = default;

                MessageBox.Show(GetInfoString(), "");
            }
        }

        static string? GetAssemblyVersion(Assembly assembly)
            => assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion
            .Split(new[] { '+', '-' }, StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault();
    }
}
