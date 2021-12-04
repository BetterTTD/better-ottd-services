namespace OpenTTD.API.Network;

public class ServerInfo
{
    public string ServerIp { get; }

    public int ServerPort { get; }

    public string Password { get; }

    public ServerInfo(string serverIp, int serverPort, string password = "")
    {
        ServerIp = serverIp;
        ServerPort = serverPort;
        Password = password;
    }

    public override string ToString() => $"{ServerIp}:{ServerPort}";
}