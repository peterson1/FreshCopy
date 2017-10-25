using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.EventHandlerTools;
using CommonTools.Lib.ns11.InputTools;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.StringTools;
using FreshCopy.Common.API.HubClients;
using System;
using System.Threading.Tasks;

namespace FreshCopy.ServerControl.WPF.ConfigEditors
{
    class ConfigEditorVM : ViewModelBase
    {
        private HubClientSession   _sess;
        private IHubSessionsClient _hub;
        private string             _key;


        public ConfigEditorVM(IHubClientSettings hubClientSettings,
                              IHubSessionsClient hubSessionsClient)
        {
            _hub             = hubSessionsClient;
            _key             = hubClientSettings.SharedKey;
            EncryptCmd       = R2Command.Relay(EncryptUnsaved, _ => !IsEncrypted, "Encrypt");
            DecryptCmd       = R2Command.Relay(DecryptUnsaved, _ => IsEncrypted, "Decrypt");
            PrettifyCmd      = R2Command.Relay(PrettifyUnsaved, _ => !IsEncrypted, "Prettify");
            SaveCmd          = R2Command.Async(SaveUnsaved, CanSave, "Save");
            PropertyChanged += ConfigEditorVM_PropertyChanged;
        }


        public string       UnsavedText  { get; set; }
        public bool         IsEncrypted  { get; private set; }
        public bool         IsEdited     { get; private set; }
        //public string       ConfigPath   { get; private set; }
        public IR2Command   EncryptCmd   { get; }
        public IR2Command   DecryptCmd   { get; }
        public IR2Command   PrettifyCmd  { get; }
        public IR2Command   SaveCmd      { get; }


        internal void Load(HubClientSession session)
        {
            _sess       = session;
            UnsavedText = _sess.JsonConfig ?? "";

            UpdateStates();
            UpdateTitle($"  {_sess}");
        }


        private bool CanSave(object arg)
        {
            if (IsBusy) return false;
            if (!IsEdited) return false;
            if (!IsEncrypted) return false;
            return true;
        }


        private async Task SaveUnsaved(object arg)
        {
            StartBeingBusy("Saving config text ...");

            var encryptd = AESThenHMAC.SimpleEncryptWithPassword(UnsavedText, _key);
            await _hub.RewriteConfigFile(encryptd, _sess.ConnectionId);

            //if (hash != UnsavedText.SHA1ForUTF8())
            //    throw new InvalidOperationException("Client may not have correctly rewritten the config file.");

            //await _hub.RequestClientState(_sess.ConnectionId);

            CloseUI();
        }


        private void UpdateStates()
        {
            IsEncrypted = !UnsavedText.TrimStart().StartsWith("{");
            IsEdited    = UnsavedText != _sess.JsonConfig;
        }


        private void ConfigEditorVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UnsavedText))
                UpdateStates();
        }

        private void DecryptUnsaved() => UnsavedText 
            = AESThenHMAC.SimpleDecryptWithPassword(UnsavedText, _key);

        private void EncryptUnsaved() => UnsavedText 
            = AESThenHMAC.SimpleEncryptWithPassword(UnsavedText, _key);


        private void PrettifyUnsaved() => UnsavedText
            = JsonFormatter.Prettify(UnsavedText);
    }
}
