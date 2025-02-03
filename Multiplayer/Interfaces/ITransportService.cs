public interface ITransportService
{
    void Update();
    void CreateServer();
    void ConnectToServer(string serverId);
    void SendPacketToServer(PacketTypes.MainType mainType, byte subType, byte[] data);
    void SendPacketToClients(PacketTypes.MainType mainType, byte subType, byte[] data);
}