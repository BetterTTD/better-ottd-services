namespace OpenTTD.Networking.AdminPort.Messages.Base;

public interface IPacketTransformer
{
    IAdminMessage Transform(Packet packet);
}