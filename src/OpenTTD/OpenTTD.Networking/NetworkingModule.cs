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
        services.AddTransient<IMessageTransformer<JoinMessage>, JoinTransformer>();
        services.AddTransient<IMessageTransformer<PollMessage>, PollTransformer>();
        services.AddTransient<IMessageTransformer<RconMessage>, RconTransformer>();
        services.AddTransient<IMessageTransformer<UpdateFrequencyMessage>, UpdateFrequencyTransformer>();

        return services;
    }

    private static IServiceCollection AddPacketTransformers(this IServiceCollection services)
    {
        services.AddTransient<IPacketTransformer<ServerClientErrorMessage>, ServerClientErrorTransformer>();
        services.AddTransient<IPacketTransformer<ServerClientInfoMessage>, ServerClientInfoTransformer>();
        services.AddTransient<IPacketTransformer<ServerClientJoinMessage>, ServerClientJoinTransformer>();
        services.AddTransient<IPacketTransformer<ServerClientQuitMessage>, ServerClientQuitTransformer>();
        services.AddTransient<IPacketTransformer<ServerClientUpdateMessage>, ServerClientUpdateTransformer>();
        
        services.AddTransient<IPacketTransformer<ServerCompanyInfoMessage>, ServerCompanyInfoTransformer>();
        services.AddTransient<IPacketTransformer<ServerCompanyNewMessage>, ServerCompanyNewTransformer>();
        services.AddTransient<IPacketTransformer<ServerCompanyRemoveMessage>, ServerCompanyRemoveTransformer>();
        services.AddTransient<IPacketTransformer<ServerCompanyUpdateMessage>, ServerCompanyUpdateTransformer>();
        
        services.AddTransient<IPacketTransformer<ServerWelcomeMessage>, ServerWelcomeTransformer>();
        services.AddTransient<IPacketTransformer<ServerProtocolMessage>, ServerProtocolTransformer>();
        
        services.AddTransient<IPacketTransformer<ServerChatMessage>, ServerChatTransformer>();
        services.AddTransient<IPacketTransformer<ServerConsoleMessage>, ServerConsoleTransformer>();
        services.AddTransient<IPacketTransformer<ServerPongMessage>, ServerPongTransformer>();
        services.AddTransient<IPacketTransformer<ServerRconMessage>, ServerRconTransformer>();

        return services;
    }
}