using System;
using Microsoft.AspNet.SignalR;

namespace SignalRChatHelloWorldMvc4
{
    public class HubClass : Hub
    {
        // server method for signalr
        public void ServerSendMessageMethod(string message)
        {
            // adding time stamp to the message 
            message += " | " + DateTime.Now.ToShortTimeString();

            // this is a client function
            Clients.All.clientReceiveMessageMethod(message);
        }
    }
}