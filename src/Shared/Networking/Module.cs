using Microsoft.Extensions.DependencyInjection;
using Networking.Messages;
using Networking.Messages.Inbound;
using Networking.Messages.Inbound.ServerChat;
using Networking.Messages.Inbound.ServerClientError;
using Networking.Messages.Inbound.ServerClientInfo;
using Networking.Messages.Inbound.ServerClientJoin;
using Networking.Messages.Inbound.ServerClientQuit;
using Networking.Messages.Inbound.ServerClientUpdate;
using Networking.Messages.Inbound.ServerCompanyInfo;
using Networking.Messages.Inbound.ServerCompanyNew;
using Networking.Messages.Inbound.ServerCompanyRemove;
using Networking.Messages.Inbound.ServerCompanyUpdate;
using Networking.Messages.Inbound.ServerConsole;
using Networking.Messages.Inbound.ServerPong;
using Networking.Messages.Inbound.ServerProtocol;
using Networking.Messages.Inbound.ServerRcon;
using Networking.Messages.Inbound.ServerWelcome;
using Networking.Messages.Outbound;
using Networking.Messages.Outbound.Join;
using Networking.Messages.Outbound.Ping;
using Networking.Messages.Outbound.Poll;
using Networking.Messages.Outbound.Rcon;
using Networking.Messages.Outbound.UpdateFrequency;

namespace Networking;

public static class Module
{
    public static IServiceCollection AddAdminPortNetworking(this IServiceCollection services)
    {
        services.AddMessageTransformers();
        services.AddPacketTransformers();
        
        services.AddTransient<IPacketService, PacketService>();
        
        return services;
    }

    private static IServiceCollection AddMessageTransformers(this IServiceCollection services)
    {
        services.AddTransient<IMessageTransformer, PingTransformer>();
        services.AddTransient<IMessageTransformer, JoinTransformer>();
        services.AddTransient<IMessageTransformer, PollTransformer>();
        services.AddTransient<IMessageTransformer, RconTransformer>();
        services.AddTransient<IMessageTransformer, UpdateFrequencyTransformer>();

        return services;
    }

    private static IServiceCollection AddPacketTransformers(this IServiceCollection services)
    {
        services.AddTransient<IPacketTransformer, ServerClientErrorTransformer>();
        services.AddTransient<IPacketTransformer, ServerClientInfoTransformer>();
        services.AddTransient<IPacketTransformer, ServerClientJoinTransformer>();
        services.AddTransient<IPacketTransformer, ServerClientQuitTransformer>();
        services.AddTransient<IPacketTransformer, ServerClientUpdateTransformer>();
        
        services.AddTransient<IPacketTransformer, ServerCompanyInfoTransformer>();
        services.AddTransient<IPacketTransformer, ServerCompanyNewTransformer>();
        services.AddTransient<IPacketTransformer, ServerCompanyRemoveTransformer>();
        services.AddTransient<IPacketTransformer, ServerCompanyUpdateTransformer>();
        
        services.AddTransient<IPacketTransformer, ServerWelcomeTransformer>();
        services.AddTransient<IPacketTransformer, ServerProtocolTransformer>();
        
        services.AddTransient<IPacketTransformer, ServerChatTransformer>();
        services.AddTransient<IPacketTransformer, ServerConsoleTransformer>();
        services.AddTransient<IPacketTransformer, ServerPongTransformer>();
        services.AddTransient<IPacketTransformer, ServerRconTransformer>();

        return services;
    }
}