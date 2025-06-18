using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DVMS.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string userId, string user_name, string message, string type)
        {
            await Clients.Others.SendAsync("broadcastMessage", userId, user_name, message, type);
        }

        public override async Task OnConnectedAsync()
        {
            string? userId = Context.GetHttpContext()?.Request.Query["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string? userId = Context.GetHttpContext()?.Request.Query["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
