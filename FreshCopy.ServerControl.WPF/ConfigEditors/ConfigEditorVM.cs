using System;
using CommonTools.Lib.ns11.StringTools;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.InputTools;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.fx45.Cryptography;
using System.Threading.Tasks;

namespace FreshCopy.ServerControl.WPF.ConfigEditors
{
    class ConfigEditorVM : ViewModelBase
    {
        private string _origCfg;
        private string _key;


        public ConfigEditorVM(IHubClientSettings hubClientSettings)
        {
            _key             = hubClientSettings.SharedKey;
            EncryptCmd       = R2Command.Relay(EncryptUnsaved, _ => !IsEncrypted, "Encrypt");
            DecryptCmd       = R2Command.Relay(DecryptUnsaved, _ => IsEncrypted, "Decrypt");
            PrettifyCmd      = R2Command.Relay(PrettifyUnsaved, _ => !IsEncrypted, "Prettify");
            SaveCmd          = R2Command.Async(SaveUnsaved,    _ => (!IsBusy) && IsEdited, "Save");
            PropertyChanged += ConfigEditorVM_PropertyChanged;
        }


        public string       UnsavedText  { get; set; }
        public bool         IsReadOnly   { get; private set; }
        public bool         IsEncrypted  { get; private set; }
        public bool         IsEdited     { get; private set; }
        public IR2Command   EncryptCmd   { get; }
        public IR2Command   DecryptCmd   { get; }
        public IR2Command   PrettifyCmd  { get; }
        public IR2Command   SaveCmd      { get; }


        internal void Load(HubClientSession session)
        {
            UpdateTitle($"  {session}");

            _origCfg = UnsavedText = session.JsonConfig ?? "";

            UpdateStates();
            IsReadOnly = IsEncrypted;
        }


        private async Task SaveUnsaved(object arg)
        {
            StartBeingBusy("Saving config text ...");
            await Task.Delay(1000 * 3);
            StopBeingBusy();
        }


        private void UpdateStates()
        {
            IsEncrypted = !UnsavedText.TrimStart().StartsWith("{");
            //IsReadOnly  = 
            IsEdited    = UnsavedText != _origCfg;
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
