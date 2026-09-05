using Avalonia;
using Avalonia.Platform;
using Avalonia.Shared.PlatformSupport;
using System.Application.Services;
using System.Application.UI.ViewModels;
using System.Application.UI.Views.Windows;
using System.IO;
using System.Windows;

namespace System.Application.UI
{
    /// <inheritdoc cref="INotificationService.NotifyIconHelper"/>
    sealed class NotifyIconHelper : INotificationService.NotifyIconHelper
    {
        private NotifyIconHelper() => throw new NotSupportedException();

        static NotifyIcon? Tray;

        static Stream GetIcon(IAssetLoader assets)
            => assets.Open(new("avares://System.Application.SteamTools.Client.Avalonia/Application/UI/Assets/Icon.ico"));

        static Stream GetIconByCurrentAvaloniaLocator()
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>()!;
            return GetIcon(assets);
        }

        public static void Init(App app, EventHandler notifyIconClick)
        {
            if (IsInitialized) return;

            var notifyIcon = DI.Get<NotifyIcon>();
            notifyIcon.Text = TaskBarWindowViewModel.TitleString;
            notifyIcon.Icon = GetIconByCurrentAvaloniaLocator();
            notifyIcon.Visible = true;
            notifyIcon.RightClick += (_, e) =>
            {
                IViewModelManager.Instance.ShowTaskBarWindow(e.X, e.Y);
            };
            notifyIcon.Click += notifyIconClick;
            notifyIcon.DoubleClick += notifyIconClick;
            notifyIcon.AddTo(app);
            Tray = notifyIcon;
            IsInitialized = true;
        }

        public static Stream GetIcon()
        {
            var assets = new AssetLoader(typeof(TaskBarWindow).Assembly);
            return GetIcon(assets);
        }

        public static void Dispoe()
        {
            if (Tray is not null)
            {
                Tray.Visible = false;
                Tray.Dispose();
                Tray = null;
            }

            IsInitialized = false;
        }
    }
}
