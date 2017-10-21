using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.InputTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Client.Lib45.HubClientStates;
using System.Threading.Tasks;
using System;
using System.Windows.Controls;

namespace FreshCopy.Client.Lib45.ProblemReporters
{
    public class ProblemReporter1VM : ViewModelBase
    {
        const string IDLE_STATUS = "Report a Problem";
        private IMessageBroadcastClient _client;
        private ClientStateComposer1    _composr;


        public ProblemReporter1VM(IMessageBroadcastClient messageBroadcastListener,
                                  ClientStateComposer1 clientStateComposer1)
        {
            _client         = messageBroadcastListener;
            _composr        = clientStateComposer1;
            ShowDialogueCmd = R2Command.Relay(ShowDialogueWindow, _ => !IsBusy, IDLE_STATUS);
            SubmitReportCmd = R2Command.Async(SendProblemReport, _ => !IsBusy, "Submit");
            SubmitReportCmd.DisableWhenDone = true;
            SetStatus(IDLE_STATUS);
        }


        public IR2Command  ShowDialogueCmd   { get; }
        public IR2Command  SubmitReportCmd   { get; }
        public string      UserNarrative     { get; set; }


        private void ShowDialogueWindow()
        {
            var win = new ProblemReportWindow1();
            win.DataContext = this;
            win.ShowDialog();
        }


        public MenuItem CreateMenuItem() => new MenuItem
        {
            Header  = IDLE_STATUS,
            Command = ShowDialogueCmd
        };


        private async Task SendProblemReport()
        {
            StartBeingBusy("Sending Problem Report ...");

            var state = await _composr.GatherClientState();
            _client.SendClientState(state);

            StartBeingBusy("Problem reported.  (closing this window ...)");
            await Task.Delay(1000 * 3);
            SetStatus(IDLE_STATUS);
            StopBeingBusy();
            CloseUI();
        }
    }
}
