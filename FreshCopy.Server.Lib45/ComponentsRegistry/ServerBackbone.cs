using Autofac;
using CommonTools.Lib.fx45.DependencyInjection;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.SignalRServers;
using CommonTools.Lib.fx45.UserControls.AppUpdateNotifiers;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.FileSystemTools;
using CommonTools.Lib.ns11.SignalRServers;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Server.Lib45.Configuration;
using FreshCopy.Server.Lib45.FileWatchers;
using FreshCopy.Server.Lib45.SignalRHubs;
using FreshCopy.Server.Lib45.ViewModels;
using Microsoft.AspNet.SignalR;
using System.Windows;

namespace FreshCopy.Server.Lib45.ComponentsRegistry
{
    public class ServerBackbone : ServerBackboneBase<MainVersionKeeperWindowVM, VersionKeeperSettings>
    {
        public ServerBackbone(Application application) : base(application)
        {
        }


        protected override void RegisterCustomComponents(ContainerBuilder b, HubConfiguration hubCfg)
        {
            b.Hub<VersionKeeperHub1>(hubCfg);

            b.Solo<MainVersionKeeperWindowVM>();

            b.Solo<ClonedCopyExeUpdater>();
            b.Solo<AppUpdateNotifierVM>();

            b.Multi<IThrottledFileWatcher, ThrottledFileWatcher1>();
            b.Multi<BinaryFileWatcherVM>();
            b.Multi<AppendOnlyDbWatcherVM>();
        }


        protected override void SetCustomDataTemplates(Application app)
        {
            app.SetTemplate<AppUpdateNotifierVM, AppUpdateNotifierUI>();
        }


        protected override void InitializeMainVM(MainVersionKeeperWindowVM mainVM, ILifetimeScope scope)
        {
            mainVM.StartFileWatchers(scope);
        }


        protected override VersionKeeperSettings GetConfigInstance()
            => VersionKeeperCfgFile.LoadOrDefault();

        //protected override void RegisterSettingsFileInstance(ContainerBuilder b)
        //{
        //    var cfg = VersionKeeperCfgFile.LoadOrDefault();
        //    b.RegisterInstance<VersionKeeperSettings>(cfg)
        //                    .As<ISignalRServerSettings>()
        //                    .AsSelf();
        //}
    }
}
