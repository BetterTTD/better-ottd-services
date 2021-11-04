using System.Threading.Tasks;
using Admin.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Admin.Server.Hubs
{
    public sealed class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            var msg = new ReceiveMessage(user, message);
            await Clients.All.SendAsync(nameof(ReceiveMessage), msg);
        }
    }
}