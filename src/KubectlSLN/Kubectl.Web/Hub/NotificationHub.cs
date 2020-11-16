using Microsoft.AspNetCore.SignalR;

namespace Kubectl.Web.Hub
{
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public void BroadcastMessage(string message)
        {
            Clients.All.SendAsync("broadcastMessage", message);
        }
    }
}