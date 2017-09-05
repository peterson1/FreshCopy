using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Common.API.ChangeDescriptions;
using FreshCopy.Common.API.TargetUpdaters;

namespace FreshCopy.Client.Lib45.BroadcastHandlers
{
    public class BinaryFileChangeBroadcastHandlerVM : ChangeBroadcastHandlerVmBase<BinaryFileChangeInfo>
    {
        public BinaryFileChangeBroadcastHandlerVM(IMessageBroadcastListener listenr,
                                            IBinaryFileUpdater binaryFileUpdater,
                                            ContextLogListVM contextLogListVM)
            : base(listenr, contextLogListVM, binaryFileUpdater)
        {
        }
    }
}
