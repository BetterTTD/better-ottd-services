namespace OpenTTD.Networking.AdminPort.Messages.Base;

public interface IPacketTransformer<out IAdminMessage>
{
    IAdminMessage Transform(Packet packet);
}