using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.SignalRServers;
using Microsoft.AspNet.SignalR.Hubs;

namespace CommonTools.Lib.fx45.HubPipelines
{
    public class LoggerPipeline1 : HubPipelineModule
    {
        private SharedLogListVM     _log;
        private CurrentHubClientsVM _clients;

        public LoggerPipeline1(SharedLogListVM commonLogListVM,
                               CurrentHubClientsVM currentHubClientsVM)
        {
            _log     = commonLogListVM;
            _clients = currentHubClientsVM;
        }

        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            var method = context.MethodDescriptor.Name;
            Log($"client invoked: [{method}]");

            var connId = context.Hub.Context.ConnectionId;
            _clients[connId].LastHubMethod = method;

            return base.OnBeforeIncoming(context);
        }

        //protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context)
        //{
        //    Log($"invoking client method: {context.Invocation.Method}");
        //    return base.OnBeforeOutgoing(context);
        //}


        private void Log(string message) => _log.Add(message);
    }
}
