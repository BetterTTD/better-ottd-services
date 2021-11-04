using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Bot.Telegram.API.HostedServices
{
    public sealed class ChatQueueService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}