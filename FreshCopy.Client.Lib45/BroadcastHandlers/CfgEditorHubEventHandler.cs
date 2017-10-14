using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Client.Lib45.Configuration;
using System;

namespace FreshCopy.Client.Lib45.BroadcastHandlers
{
    public class CfgEditorHubEventHandler
    {
        private IMessageBroadcastClient _listnr;
        private string                  _key;


        public CfgEditorHubEventHandler(IMessageBroadcastClient messageBroadcastListener,
                                        IHubClientSettings hubClientSettings)
        {
            _key    = hubClientSettings.SharedKey;
            _listnr = messageBroadcastListener;
            _listnr.ConfigRewriteRequested += OnConfigRewriteRequested;
        }


        private void OnConfigRewriteRequested(object sender, string encryptedDTO)
        {
            try
            {
                UpdateCheckerCfgFile.RewriteWith(encryptedDTO, _key);
            }
            catch (Exception ex)
            {
                _listnr.SendException("OnConfigRewriteRequested", ex);
            }
        }
    }
}
