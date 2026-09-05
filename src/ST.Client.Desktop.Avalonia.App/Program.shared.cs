using System.Application.CommandLine;
using System.Application.Services;

namespace System.Application.UI;

static partial class Program
{
    private sealed partial class ProgramHost
    {
        public static ProgramHost Instance { get; } = new();

        private ProgramHost()
        {

        }
    }

    partial class ProgramHost : IApplication.IDesktopProgramHost
    {
        public bool IsMinimize { get; set; }

        public bool IsCLTProcess { get; set; }

        public bool IsMainProcess { get; set; }

        public bool IsTrayProcess { get; set; }

        public bool IsProxy { get; set; }

        public EOnOff ProxyStatus { get; set; }

        void IApplication.IProgramHost.ConfigureServices(DILevel level, bool isTrace) => ConfigureServices(level, isTrace);

        public void InitVisualStudioAppCenterSDK()
        {
#pragma warning disable IDE0079 // 请删除不必要的忽略
#pragma warning disable CA1416 // 验证平台兼容性
            VisualStudioAppCenterSDK.Init();
#pragma warning restore CA1416 // 验证平台兼容性
#pragma warning restore IDE0079 // 请删除不必要的忽略
        }

        public void OnStartup() => Program.OnStartup(this);

        IApplication IApplication.IProgramHost.Application => App.Instance;

        void IApplication.IDesktopProgramHost.OnCreateAppExecuted(Action<IViewModelManager>? handlerViewModelManager, bool isTrace) => Program.OnCreateAppExecuted(this, handlerViewModelManager, isTrace);

        DeploymentMode IApplication.IDesktopProgramHost.DeploymentMode => DeploymentMode.
#if FRAMEWORK_DEPENDENT || !PUBLISH
           FDE
#else
           SCD
#endif
           ;
    }

    partial class ProgramHost : CommandLineHost
    {
        protected override IApplication.IDesktopProgramHost Host => this;

        public Action<DILevel>? ConfigureServicesDelegate { get; set; }

        protected override void ConfigureServices(DILevel level, bool isTrace = false)
        {
            if (ConfigureServicesDelegate != null)
            {
                ConfigureServicesDelegate.Invoke(level);
            }
            else
            {
                Program.ConfigureServices(this, level, isTrace: isTrace);
            }
        }

#if StartWatchTrace
        protected override void StartWatchTraceRecord(string? mark = null, bool dispose = false)
            => StartWatchTrace.Record(mark, dispose);
#endif

        protected override void StartApplication(string[] args) => StartAvaloniaApp(args);

        public override IApplication? Application => App.Instance;

        protected override void SetIsMainProcess(bool value) => IsMainProcess = value;

        protected override void SetIsCLTProcess(bool value) => IsCLTProcess = value;
    }
}
