using System.Threading.Tasks;
using Admin.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Admin.Server.Hubs
{
    public sealed class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message) => 
            await Clients.All.SendAsync(
                nameof(ReceiveMessage), 
                new ReceiveMessage(user, message));
    }
}