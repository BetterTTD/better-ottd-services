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
using OpenTTD.Networking.Messages.Outbound.Poll;
using OpenTTD.Networking.Messages.Outbound.Rcon;
using OpenTTD.Networking.Messages.Outbound.UpdateFrequency;

namespace OpenTTD.Networking;

public static class NetworkingModule
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