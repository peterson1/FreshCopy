﻿using CommonTools.Lib.ns11.CollectionTools;
using CommonTools.Lib.ns11.InputTools;
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
            root.Items.Add(CreateLabelItem("Version Updater"));

            AddExecutableMenuItemsTo(root);

            root.Items.Add(new Separator());

            root.Items.Add(_reportr.CreateMenuItem());

            //root.Items.Add(CreateExitMenuItem(vm));
            root.Items.Add(NewMenuItem("Relaunch Updater", vm.RelaunchCmd));
            root.Items.Add(NewMenuItem("Exit Updater"    , vm.ExitCmd));
        }


        private void AddExecutableMenuItemsTo(ContextMenu root)
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
                _client.SendException("Set Menu Items for Exe", ex);
            }
        }


        //private MenuItem CreateExitMenuItem(MainCheckerWindowVM vm) => new MenuItem
        //{
        //    Header  = "Exit",
        //    Command = vm.ExitCmd
        //};
        private MenuItem NewMenuItem(string header, IR2Command command) => new MenuItem
        {
            Header  = header,
            Command = command
        };


        internal void SetLatestVersion(string fileKey, string latestVersion)
            => _latestVer[fileKey] = latestVersion;


        //todo: DRY this up
        private static MenuItem CreateLabelItem(string header) => new MenuItem
        {
            Header = header,
            IsEnabled = false
        };
    }
}
