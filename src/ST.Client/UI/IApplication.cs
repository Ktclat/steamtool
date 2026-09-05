using System.Application.Models;
using System.Application.Services;
using System.Application.Settings;
using System.Application.UI.Resx;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace System.Application.UI
{
    /// <summary>
    /// 当前应用程序
    /// </summary>
    public partial interface IApplication
    {
        /// <summary>
        /// This edition always runs on Windows desktop.
        /// </summary>
        [SupportedOSPlatformGuard("Windows7.0")]
        const bool IsDesktopPlatform = true;

        static IApplication Instance => DI.Get<IApplication>();

        /// <summary>
        /// 获取或设置当前应用的主题
        /// </summary>
        AppTheme Theme { get; set; }

        /// <summary>
        /// 获取当前应用的实际主题
        /// </summary>
        AppTheme ActualTheme => Theme switch
        {
            AppTheme.FollowingSystem => GetActualThemeByFollowingSystem(),
            AppTheme.Light => AppTheme.Light,
            AppTheme.Dark => AppTheme.Dark,
            _ => DefaultActualTheme,
        };

        /// <summary>
        /// 获取当前应用的默认主题
        /// </summary>
        protected AppTheme DefaultActualTheme { get; }

        static AppTheme GetAppThemeByIsLightOrDarkTheme(bool isLightOrDarkTheme) => isLightOrDarkTheme ? AppTheme.Light : AppTheme.Dark;

        /// <summary>
        /// 获取当前应用主题跟随系统时的实际主题
        /// </summary>
        /// <returns></returns>
        protected AppTheme GetActualThemeByFollowingSystem()
        {
            var dps = IPlatformService.Instance;
            var isLightOrDarkTheme = dps.IsLightOrDarkTheme;
            if (isLightOrDarkTheme.HasValue)
            {
                return GetAppThemeByIsLightOrDarkTheme(isLightOrDarkTheme.Value);
            }
            return DefaultActualTheme;
        }

        /// <summary>
        /// 获取当前平台 UI Host
        /// <para>Reference to the current Windows desktop host.</para>
        /// </summary>
        object CurrentPlatformUIHost { get; }

        /// <summary>
        /// 初始化设置项变更时监听
        /// </summary>
        void InitSettingSubscribe()
        {
            UISettings.Theme.Subscribe(x => Theme = (AppTheme)x);
            UISettings.Language.Subscribe(R.ChangeLanguage);
        }

        /// <inheritdoc cref="IApplication.InitSettingSubscribe"/>
        void PlatformInitSettingSubscribe() => InitSettingSubscribe();

        DeploymentMode DeploymentMode => DeploymentMode.SCD;

        /// <summary>
        /// 通用复制字符串到剪贴板，并在成功后显示 toast
        /// </summary>
        /// <param name="text"></param>
        /// <param name="msgToast"></param>
        /// <param name="showToast"></param>
        /// <returns></returns>
        static async Task CopyToClipboardAsync(string? text, string? msgToast = null, bool showToast = true)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                await Clipboard2.SetTextAsync(text);
                if (showToast) Toast.Show(msgToast ?? AppResources.CopyToClipboard);
            }
        }
    }
}
