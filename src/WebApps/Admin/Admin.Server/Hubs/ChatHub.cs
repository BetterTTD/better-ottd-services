using System.Threading.Tasks;
using Admin.Shared;
using EventBus.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace Admin.Server.Hubs
{
    public sealed class ChatHub : Hub
    {
        private readonly IEventBus _bus;

        public ChatHub(IEventBus bus) => _bus = bus;

        public async Task SendMessage(string user, string message)
        {
            var @event = new ReceiveMessage(user, message);
            _bus.Publish(@event);
            await Clients.All.SendAsync(nameof(ReceiveMessage), @event);
        }
    }
}