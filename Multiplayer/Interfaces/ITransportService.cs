public interface ITransportService
{
    void Update();
    void Disconnect();
    bool CreateServer();
    bool ConnectToServer(string serverId);
    void SendReliablePacketToServer(byte mainType, byte subType, byte playerIndex, byte[] data);
    void SendUnreliablePacketToServer(byte mainType, byte subType, byte playerIndex, ushort tick, byte[] data);
    void SendReliablePacketToClients(byte mainType, byte subType, byte playerIndex, byte[] data);
    void SendUnreliablePacketToClients(byte mainType, byte subType, byte playerIndex, ushort tick, byte[] data);
}