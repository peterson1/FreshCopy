﻿using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.ns11.DataStructures;
using CommonTools.Lib.ns11.SignalRClients;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace FreshCopy.Server.Lib45.HubClientStates
{
    public class AuthorizeHelperV1
    {
        private CurrentHubClientsVM _clients;


        public AuthorizeHelperV1(CurrentHubClientsVM activeHubClientsList)
        {
            _clients = activeHubClientsList;
        }


        public string TargetHubName { get; set; }


        public async Task Enlist(HubCallerContext context)
        {
            await Task.Delay(0);
            Enlist(context, null);
        }


        public void Enlist(HubCallerContext context,
                           CurrentClientState clientState)
        {
            if (!IsValidSession(context, out HubClientSession session)) return;

            if (clientState != null)
                session.CurrentState = clientState;

            _clients.AddOrUpdate(session);
        }


        public void AddError(HubCallerContext context, ExceptionReport exceptionReport)
        {
            if (!IsValidSession(context, out HubClientSession session)) return;
            session.Errors.Add(exceptionReport);
            //_clients.NotifyClientInteracted(session);
            _clients.AddOrUpdate(session);
        }


        public async Task Delist(HubCallerContext context, bool stopCalled)
        {
            await Task.Delay(0);
            _clients.Remove(context.ConnectionId);
        }


        private bool IsValidSession(HubCallerContext context, out HubClientSession session)
        {
            if (!context.TryGetSession(out session)) return false;
            session.HubName = TargetHubName;
            session.HubClientIP = context.Request.Environment["server.RemoteIpAddress"].ToString();
            return true;
        }
    }
}
