﻿using CommonTools.Lib.fx45.ImagingTools;
using CommonTools.Lib.fx45.UserControls;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.ServerControl.WPF.ConfigEditors;
using System.Windows;
using System.Windows.Controls;

namespace FreshCopy.ServerControl.WPF.CurrentClientele
{
    public partial class SubTableByUserAgent : UserControl
    {
        public SubTableByUserAgent()
        {
            InitializeComponent();

            Loaded += (s, e) => dg.EnableToggledColumns(DataGridLengthUnitType.Auto);
        }

        private void screenShow_Click(object sendr, RoutedEventArgs e)
        {
            if (!TryGetSession(sendr, out HubClientSession sess)) return;
            //var b64  = sess.CurrentState?.ScreenshotB64;
            //if (b64.IsBlank()) return;
            //var bmp = CreateBitmap.FromBase64(b64);
            var cap = $"[timestamp should be here]  {sess.UserAgent}";
            BitmapWindow1.Show(cap, sess.CurrentState?.ScreenshotB64);
        }

        private void cfgEdit_Click(object sendr, RoutedEventArgs e)
        {
            if (!TryGetSession(sendr, out HubClientSession sess)) return;
            //Alert.Show(sess.JsonConfig);
            CfgEditorWindow1.Show(sess);
        }


        private bool TryGetSession(object sender, out HubClientSession session)
        {
            var btn = sender as Button;
            session = btn?.CommandParameter as HubClientSession;
            return session != null;
        }
    }
}
