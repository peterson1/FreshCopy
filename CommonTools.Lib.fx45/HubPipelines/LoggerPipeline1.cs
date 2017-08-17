using CommonTools.Lib.fx45.ViewModelTools;
using Microsoft.AspNet.SignalR.Hubs;

namespace CommonTools.Lib.fx45.HubPipelines
{
    public class LoggerPipeline1 : HubPipelineModule
    {
        private CommonLogListVM _log;

        public LoggerPipeline1(CommonLogListVM commonLogListVM)
        {
            _log = commonLogListVM;
        }

        protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context)
        {
            _log.Add($"method: {context.Invocation.Method}");
            return base.OnBeforeOutgoing(context);
        }
    }
}
