using System;
using System.Runtime.InteropServices;
using GodotPeer2PeerSteamCSharp.Core;
using GodotPeer2PeerSteamCSharp.Core.Lobby;
using Steamworks;
using Steamworks.Data;

public class TransportService : ITransportService
{
    private static SocketManager serverSocket;
    private static ClientConnectionManager clientConnection;
    private static ServerCallbacks serverCallbacks;

    private Action _updateMethod;

    public TransportService()
    {
        serverCallbacks = new ServerCallbacks();
    }

    public void Update()
    {
        _updateMethod();
    }

    public void SetUpdateMethod(string updateMethod)
    {
        if (updateMethod == "ServerUpdate")
        {
            _updateMethod = ServerUpdate;
            return;
        }

        _updateMethod = ClientUpdate;
    }

    public bool IsConnectionActive()
    {
        return clientConnection.Connected || serverSocket != null;
    }

    public void CreateServer()
    {
        serverSocket = SteamNetworkingSockets.CreateRelaySocket<SocketManager>();

        if (serverSocket == null)
            throw new Exception("Failed to create Steam relay server.");

        LobbyManager.Instance.SetServerId(SteamClient.SteamId.ToString());
        _updateMethod = ServerUpdate;
        TransportManager.Instance.ExecuteProcessMethodStatus(true);
        serverSocket.Interface = serverCallbacks;
    }

    public void ConnectToServer(string serverId)
    {
        var steamIdValue = ulong.Parse(serverId);
        var hostSteamId = new SteamId { Value = steamIdValue };
        clientConnection = SteamNetworkingSockets.ConnectRelay<ClientConnectionManager>(hostSteamId);
    }

    public void Disconnect()
    {
        if (serverSocket != null)
        {
            serverSocket.Close();
            serverSocket = null;
            return;
        }

        clientConnection?.Close();
        clientConnection = null;
    }

    public void CreateAndSendReliablePacketToServer(byte mainType, byte subType, byte playerIndex, Span<byte> data)
    {
        var packet = PacketFactory.CreateReliablePacket(mainType, subType, playerIndex, data, out var totalSize);
        clientConnection?.Connection.SendMessage(packet, totalSize);
        Marshal.FreeHGlobal(packet);
    }

    public void CreateAndSendUnreliablePacketToServer(byte mainType, byte subType, byte playerIndex, ushort tick,
        Span<byte> data)
    {
        var packetPtr =
            PacketFactory.CreateUnreliablePacket(mainType, subType, playerIndex, tick, data, out var totalSize);
        clientConnection?.Connection.SendMessage(packetPtr, totalSize, SendType.Unreliable);
        Marshal.FreeHGlobal(packetPtr);
    }

    public void CreateAndSendReliablePacketToClients(byte mainType, byte subType, byte playerIndex, Span<byte> data)
    {
        var packetPtr = PacketFactory.CreateReliablePacket(mainType, subType, playerIndex, data, out var totalSize);
        SendReliablePacketToClients(packetPtr, totalSize);
    }

    public void CreateAndSendUnreliablePacketToClients(byte mainType, byte subType, byte playerIndex, ushort tick,
        Span<byte> data)
    {
        var packet =
            PacketFactory.CreateUnreliablePacket(mainType, subType, playerIndex, tick, data, out var totalSize);
        SendUnreliablePacketToClients(packet, totalSize);
    }

    private void ServerUpdate()
    {
        serverSocket.Receive();
    }

    private void ClientUpdate()
    {
        clientConnection.Receive();
    }

    private static void SendReliablePacketToClients(IntPtr packetPtr, int totalSize)
    {
        if (serverSocket == null)
            return;

        foreach (var client in serverSocket.Connected)
            client.SendMessage(packetPtr, totalSize);

        Marshal.FreeHGlobal(packetPtr);
    }

    private static void SendUnreliablePacketToClients(IntPtr packetPtr, int totalSize)
    {
        if (serverSocket == null)
            return;

        foreach (var client in serverSocket.Connected)
            client.SendMessage(packetPtr, totalSize, SendType.Unreliable);

        Marshal.FreeHGlobal(packetPtr);
    }

    public static void RelayPacketToClients(Connection originConnection, IntPtr packetPtr, int totalSize,
        SendType sendType)
    {
        if (serverSocket == null)
            return;

        foreach (var client in serverSocket.Connected)
            if (client != originConnection)
                client.SendMessage(packetPtr, totalSize, sendType);
    }
}

public class ServerCallbacks : ISocketManager
{
    public void OnConnecting(Connection connection, ConnectionInfo data)
    {
        connection.Accept();
        Logger.Network($"Server: Player {data.Identity} is attempting to connect...");
    }

    public void OnConnected(Connection connection, ConnectionInfo data)
    {
        Logger.Network($"Server: Player {data.Identity} has successfully connected!");
    }

    public void OnDisconnected(Connection connection, ConnectionInfo data)
    {
        Logger.Network($"Server: Player {data.Identity} has disconnected.");
    }

    public unsafe void OnMessage(Connection connection, NetIdentity identity, IntPtr data, int size, long messageNum,
        long recvTime, int channel)
    {
        var span = new Span<byte>((void*) data, size);

        var mainType = span[0];
        var subType = span[1];
        var playerIndex = span[2];
        var sendType = span[3];

        // Reliable
        if (sendType == 0)
        {
            TransportService.RelayPacketToClients(connection, data, size, SendType.Reliable);
            var payload = span.Slice(PacketFactory.HeaderSizeReliable, size - PacketFactory.HeaderSizeReliable);
            TransportManager.Server.OnReliablePacketReceived(mainType, subType, playerIndex, payload);
        }

        // Unreliable
        else
        {
            TransportService.RelayPacketToClients(connection, data, size, SendType.Unreliable);
            var tick = BitConverter.ToUInt16(span.Slice(PacketFactory.HeaderSizeReliable, 2));
            var payload = span.Slice(PacketFactory.HeaderSizeUnreliable, size - PacketFactory.HeaderSizeUnreliable);
            TransportManager.Server.OnUnreliablePacketReceived(mainType, subType, playerIndex, tick, payload);
        }
    }
}

public class ClientConnectionManager : ConnectionManager
{
    public override void OnConnecting(ConnectionInfo info)
    {
        Logger.Network("Client: Attempting to connect to server...");
    }

    public override void OnConnected(ConnectionInfo info)
    {
        TransportManager.Client.OnSuccessfulConnection();
    }

    public override void OnDisconnected(ConnectionInfo info)
    {
        Logger.Network("Client: Disconnected from server.");
        TransportManager.Instance.ExecuteProcessMethodStatus(false);
    }

    public override unsafe void OnMessage(IntPtr data, int size, long messageNum, long recvTime, int channel)
    {
        var span = new Span<byte>((void*) data, size);

        var mainType = span[0];
        var subType = span[1];
        var playerIndex = span[2];
        var sendType = span[3];

        // Reliable
        if (sendType == 0)
        {
            var payload = span.Slice(PacketFactory.HeaderSizeReliable, size - PacketFactory.HeaderSizeReliable);
            TransportManager.Client.OnReliablePacketReceived(mainType, subType, playerIndex, payload);
        }

        // Unreliable
        else
        {
            var tick = BitConverter.ToUInt16(span.Slice(PacketFactory.HeaderSizeReliable, 2));
            var payload = span.Slice(PacketFactory.HeaderSizeUnreliable, size - PacketFactory.HeaderSizeUnreliable);
            TransportManager.Client.OnUnreliablePacketReceived(mainType, subType, playerIndex, tick, payload);
        }
    }
}
