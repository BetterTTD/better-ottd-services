using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Bot.Telegram.API.HostedServices
{
    public class ConfigureBotWebhook : IHostedService
    {
        private readonly ILogger<ConfigureBotWebhook> _logger;
        private readonly IServiceProvider _services;

        private readonly BotConfiguration _botConfig;

        public ConfigureBotWebhook(
            ILogger<ConfigureBotWebhook> logger, 
            IServiceProvider services, 
            IConfiguration configuration)
        {
            _logger = logger;
            _services = services;

            _botConfig = configuration
                .GetSection(BotConfiguration.Position)
                .Get<BotConfiguration>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            var webhookAddress = @$"{_botConfig.HostAddress}/bot/{_botConfig.BotToken}";
            _logger.LogInformation("Setting webhook: {WebhookAddress}", webhookAddress);
            await botClient.SetWebhookAsync(
                webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            _logger.LogInformation("Removing webhook");
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }

    public sealed class BotConfiguration
    {
        public const string Position = nameof(BotConfiguration);
        public string HostAddress { get; set; } = string.Empty;
        public string BotToken { get; set; } = string.Empty;
    }
}