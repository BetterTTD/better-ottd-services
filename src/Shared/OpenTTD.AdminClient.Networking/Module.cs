using Microsoft.Extensions.DependencyInjection;
using OpenTTD.AdminClient.Networking.Messages;
using OpenTTD.AdminClient.Networking.Messages.Inbound;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerChat;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerClientError;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerClientInfo;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerClientJoin;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerClientQuit;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerClientUpdate;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerCompanyInfo;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerCompanyNew;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerCompanyRemove;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerCompanyUpdate;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerConsole;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerPong;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerProtocol;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerRcon;
using OpenTTD.AdminClient.Networking.Messages.Inbound.ServerWelcome;
using OpenTTD.AdminClient.Networking.Messages.Outbound;
using OpenTTD.AdminClient.Networking.Messages.Outbound.Join;
using OpenTTD.AdminClient.Networking.Messages.Outbound.Ping;
using OpenTTD.AdminClient.Networking.Messages.Outbound.Poll;
using OpenTTD.AdminClient.Networking.Messages.Outbound.Rcon;
using OpenTTD.AdminClient.Networking.Messages.Outbound.UpdateFrequency;

namespace OpenTTD.AdminClient.Networking;

public static class Module
{
    public static IServiceCollection AddAdminPortNetworking(this IServiceCollection services) => services
        .AddMessageTransformers()
        .AddPacketTransformers()
        .AddTransient<IPacketService, PacketService>()
        .AddTransient<IMessageDeserializer, MessageDeserializer>();

    private static IServiceCollection AddMessageTransformers(this IServiceCollection services) => services
        .AddTransient<IMessageTransformer, PingTransformer>()
        .AddTransient<IMessageTransformer, JoinTransformer>()
        .AddTransient<IMessageTransformer, PollTransformer>()
        .AddTransient<IMessageTransformer, RconTransformer>()
        .AddTransient<IMessageTransformer, UpdateFrequencyTransformer>();

    private static IServiceCollection AddPacketTransformers(this IServiceCollection services) => services
        .AddTransient<IPacketTransformer, ServerClientErrorTransformer>()
        .AddTransient<IPacketTransformer, ServerClientInfoTransformer>()
        .AddTransient<IPacketTransformer, ServerClientJoinTransformer>()
        .AddTransient<IPacketTransformer, ServerClientQuitTransformer>()
        .AddTransient<IPacketTransformer, ServerClientUpdateTransformer>()

        .AddTransient<IPacketTransformer, ServerCompanyInfoTransformer>()
        .AddTransient<IPacketTransformer, ServerCompanyNewTransformer>()
        .AddTransient<IPacketTransformer, ServerCompanyRemoveTransformer>()
        .AddTransient<IPacketTransformer, ServerCompanyUpdateTransformer>()

        .AddTransient<IPacketTransformer, ServerWelcomeTransformer>()
        .AddTransient<IPacketTransformer, ServerProtocolTransformer>()

        .AddTransient<IPacketTransformer, ServerChatTransformer>()
        .AddTransient<IPacketTransformer, ServerConsoleTransformer>()
        .AddTransient<IPacketTransformer, ServerPongTransformer>()
        .AddTransient<IPacketTransformer, ServerRconTransformer>();
}