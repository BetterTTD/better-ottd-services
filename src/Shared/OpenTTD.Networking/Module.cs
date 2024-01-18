using Microsoft.Extensions.DependencyInjection;
using OpenTTD.Networking.Messages;
using OpenTTD.Networking.Messages.Inbound;
using OpenTTD.Networking.Messages.Inbound.ServerChat;
using OpenTTD.Networking.Messages.Inbound.ServerClientError;
using OpenTTD.Networking.Messages.Inbound.ServerClientInfo;
using OpenTTD.Networking.Messages.Inbound.ServerClientJoin;
using OpenTTD.Networking.Messages.Inbound.ServerClientQuit;
using OpenTTD.Networking.Messages.Inbound.ServerClientUpdate;
using OpenTTD.Networking.Messages.Inbound.ServerCompanyInfo;
using OpenTTD.Networking.Messages.Inbound.ServerCompanyNew;
using OpenTTD.Networking.Messages.Inbound.ServerCompanyRemove;
using OpenTTD.Networking.Messages.Inbound.ServerCompanyUpdate;
using OpenTTD.Networking.Messages.Inbound.ServerConsole;
using OpenTTD.Networking.Messages.Inbound.ServerPong;
using OpenTTD.Networking.Messages.Inbound.ServerProtocol;
using OpenTTD.Networking.Messages.Inbound.ServerRcon;
using OpenTTD.Networking.Messages.Inbound.ServerWelcome;
using OpenTTD.Networking.Messages.Outbound;
using OpenTTD.Networking.Messages.Outbound.Join;
using OpenTTD.Networking.Messages.Outbound.Ping;
using OpenTTD.Networking.Messages.Outbound.Poll;
using OpenTTD.Networking.Messages.Outbound.Rcon;
using OpenTTD.Networking.Messages.Outbound.UpdateFrequency;

namespace OpenTTD.Networking;

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