namespace OpenTTD.Network.Models;

public class Player
{
    public uint ClientId { get; }
    public string Name { get; set; }

    public Player(uint clientId) => ClientId = clientId;

    public Player(uint clientId, string name)
        :this(clientId) => Name = name;

}