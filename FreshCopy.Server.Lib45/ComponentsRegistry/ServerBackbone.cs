﻿using Autofac;
using CommonTools.Lib.fx45.DependencyInjection;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.HubPipelines;
using CommonTools.Lib.fx45.SignalRServers;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.FileSystemTools;
using CommonTools.Lib.ns11.SignalRHubServers;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Server.Lib45.Configuration;
using FreshCopy.Server.Lib45.FileWatchers;
using FreshCopy.Server.Lib45.ViewModels;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using System.Windows;

namespace FreshCopy.Server.Lib45.ComponentsRegistry
{
    public class ServerBackbone : ServerBackboneBase<MainVersionKeeperWindowVM>
    {
        public ServerBackbone(Application application) : base(application)
        {
        }


        protected override void RegisterCustomComponents(ContainerBuilder b, HubConfiguration hubCfg)
        {
            b.Solo<MainVersionKeeperWindowVM>();

            b.Multi<IThrottledFileWatcher, ThrottledFileWatcher1>();
            b.Multi<BinaryFileWatcherVM>();
        }


        protected override void InitializeMainVM(MainVersionKeeperWindowVM mainVM, ILifetimeScope scope)
        {
            mainVM.StartFileWatchers(scope);
        }


        protected override void RegisterSettingsFileInstance(ContainerBuilder b)
        {
            var cfg = VersionKeeperCfgFile.LoadOrDefault();
            b.RegisterInstance<VersionKeeperSettings>(cfg)
                            .As<ISignalRServerSettings>()
                            .AsSelf();
        }
    }
}