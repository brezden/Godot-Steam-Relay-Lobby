public interface ITransportService
{
    void Update();
    void Disconnect();
    bool CreateServer();
    bool ConnectToServer(string serverId);
    void SendReliablePacketToServer(PacketTypes.MainType mainType, byte subType, byte playerIndex, byte[] data);
    void SendUnreliablePacketToServer(PacketTypes.MainType mainType, byte subType, byte playerIndex, ushort tick, byte[] data);
    void SendReliablePacketToClients(PacketTypes.MainType mainType, byte subType, byte playerIndex, byte[] data);
    void SendUnreliablePacketToClients(PacketTypes.MainType mainType, byte subType, byte playerIndex, ushort tick, byte[] data);
}