using System;

public interface ITransportService
{
    void Update();
    void Disconnect();
    bool CreateServer();
    bool ConnectToServer(string serverId);
    void CreateAndSendReliablePacketToServer(byte mainType, byte subType, byte playerIndex, byte[] data);
    void CreateAndSendUnreliablePacketToServer(byte mainType, byte subType, byte playerIndex, ushort tick, byte[] data);
    void CreateAndSendReliablePacketToClients(byte mainType, byte subType, byte playerIndex, byte[] data);
    void CreateAndSendUnreliablePacketToClients(byte mainType, byte subType, byte playerIndex, ushort tick, byte[] data);
    void SendReliablePacketToClient(IntPtr packetPtr, int totalSize);
    void SendUnreliablePacketToClient(IntPtr packetPtr, int totalSize);
}