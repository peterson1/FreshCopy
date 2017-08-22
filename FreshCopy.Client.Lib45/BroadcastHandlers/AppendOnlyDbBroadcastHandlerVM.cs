using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Common.API.ChangeDescriptions;
using FreshCopy.Common.API.TargetUpdaters;

namespace FreshCopy.Client.Lib45.BroadcastHandlers
{
    public class AppendOnlyDbBroadcastHandlerVM : BroadcastHandlerVmBase<AppendOnlyDbChangeInfo>
    {
        public AppendOnlyDbBroadcastHandlerVM(IMessageBroadcastListener listenr,
                                              IAppendOnlyDbUpdater appendOnlyDbUpdater,
                                              ContextLogListVM contextLogListVM) 
            : base(listenr, contextLogListVM, appendOnlyDbUpdater)
        {
        }
    }
}
