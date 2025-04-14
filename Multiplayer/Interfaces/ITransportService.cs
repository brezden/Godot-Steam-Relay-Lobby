using System;

public interface ITransportService
{
    void Update();
    void Disconnect();
    bool CreateServer();
    bool ConnectToServer(string serverId);
    void CreateAndSendReliablePacketToServer(byte mainType, byte subType, byte playerIndex, Span<byte> data);
    void CreateAndSendUnreliablePacketToServer(byte mainType, byte subType, byte playerIndex, ushort tick, Span<byte> data);
    void CreateAndSendReliablePacketToClients(byte mainType, byte subType, byte playerIndex, Span<byte> data);
    void CreateAndSendUnreliablePacketToClients(byte mainType, byte subType, byte playerIndex, ushort tick, Span<byte> data);
}