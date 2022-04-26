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
        services.AddTransient(typeof(IMessageTransformer<>), typeof(JoinTransformer));
        services.AddTransient(typeof(IMessageTransformer<>), typeof(PollTransformer));
        services.AddTransient(typeof(IMessageTransformer<>), typeof(RconTransformer));
        services.AddTransient(typeof(IMessageTransformer<>), typeof(UpdateFrequencyTransformer));

        return services;
    }

    private static IServiceCollection AddPacketTransformers(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerClientErrorTransformer));
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerClientInfoTransformer));
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerClientJoinTransformer));
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerClientQuitTransformer));
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerClientUpdateTransformer));
        
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerCompanyInfoTransformer));
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerCompanyNewTransformer));
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerCompanyRemoveTransformer));
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerCompanyUpdateTransformer));
        
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerWelcomePacketTransformer));
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerProtocolTransformer));
        
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerChatTransformer));
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerConsoleTransformer));
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerPongTransformer));
        services.AddTransient(typeof(IPacketTransformer<>), typeof(ServerRconTransformer));

        return services;
    }
}