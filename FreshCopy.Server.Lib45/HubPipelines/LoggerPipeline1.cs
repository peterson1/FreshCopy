﻿using CommonTools.Lib.fx45.LoggingTools;
using FreshCopy.Server.Lib45.HubClientStates;
using Microsoft.AspNet.SignalR.Hubs;

namespace FreshCopy.Server.Lib45.HubPipelines
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
            //Log($"client invoked: [{method}]");

            //var connId = context.Hub.Context.ConnectionId;
            var client = _clients[context.Hub.Context.ConnectionId];
            client.LastHubMethod = method;
            //client.HubClientIP   = GetHubClientIP(context);
            client.Logs.Add($"invoked: [{method}]");

            return base.OnBeforeIncoming(context);
        }


        //private string GetHubClientIP(IHubIncomingInvokerContext context)
        //{
        //    try
        //    {
        //        //return context?.Hub?.Context?.Request?.Environment["server.RemoteIpAddress"]?.ToString();
        //        return context == null ? "context == null"
        //             : context.Hub == null ? "context.Hub == null"
        //             : context.Hub.Context == null ? "context.Hub.Context == null"
        //             : context.Hub.Context.Request == null ? "context.Hub.Context.Request == null"
        //             : context.Hub.Context.Request.Environment == null ? "context.Hub.Context.Request.Environment == null"
        //             : context.Hub.Context.Request.Environment["server.RemoteIpAddress"] == null ? "context.Hub.Context.Request.Environment['server.RemoteIpAddress'] == null"
        //             : context.Hub.Context.Request.Environment["server.RemoteIpAddress"].ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Info(true, true);
        //    }
        //}


        //protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context)
        //{
        //    Log($"invoking client method: {context.Invocation.Method}");
        //    return base.OnBeforeOutgoing(context);
        //}


        private void Log(string message) => _log.Add(message);
    }
}
