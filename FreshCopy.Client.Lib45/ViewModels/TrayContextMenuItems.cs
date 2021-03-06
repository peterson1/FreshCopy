﻿using CommonTools.Lib.fx45.UIExtensions;
using CommonTools.Lib.ns11.CollectionTools;
using CommonTools.Lib.ns11.LoggingTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Client.Lib45.ProblemReporters;
using FreshCopy.Common.API.Configuration;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FreshCopy.Client.Lib45.ViewModels
{
    public class TrayContextMenuItems
    {
        private UpdateCheckerSettings      _cfg;
        private IMessageBroadcastClient    _client;
        private ProblemReporter1VM         _reportr;
        private Dictionary<string, string> _latestVer = new Dictionary<string, string>();


        public TrayContextMenuItems(UpdateCheckerSettings updateCheckerSettings,
                                    IMessageBroadcastClient messageBroadcastClient,
                                    ProblemReporter1VM problemReporter1VM)
        {
            _cfg     = updateCheckerSettings;
            _client  = messageBroadcastClient;
            _reportr = problemReporter1VM;
        }


        internal void SetItems(ContextMenu root, MainCheckerWindowVM vm)
        {
            //while (root.Items.Count > 1)
            //    root.Items.RemoveAt(1);
            root.Items.Clear();
            root.Items.AddDisabledItem("Version Updater");

            AddExecutableMenuItemsTo(root);

            root.Items.Add(new Separator());

            root.Items.Add(_reportr.CreateMenuItem());

            //root.Items.Add(CreateExitMenuItem(vm));
            root.Items.AddCommandItem("Relaunch Updater", vm.RelaunchCmd);
            root.Items.AddCommandItem("Exit Updater"    , vm.ExitCmd);
        }


        private async void AddExecutableMenuItemsTo(ContextMenu root)
        {
            try
            {
                foreach (var exe in _cfg.Executables)
                {
                    var ver = _latestVer.GetOrDefault(exe.Key);
                    root.Items.Add(ExeMenuItems.CreateGroup(exe, ver));
                }
            }
            catch (Exception ex)
            {
                //_client.SendException("Set Menu Items for Exe", ex);
                await Loggly.Post(ex);
            }
        }


        internal void SetLatestVersion(string fileKey, string latestVersion)
            => _latestVer[fileKey] = latestVersion;
    }
}
