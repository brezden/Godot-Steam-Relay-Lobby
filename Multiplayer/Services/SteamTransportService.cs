using System;
using Godot;
using Steamworks;
using Steamworks.Data;
using System.Buffers;
using System.Runtime.InteropServices;

public class SteamTransportService : ITransportService
{
    private SocketManager serverSocket;
    private ClientConnectionManager clientConnection;
    private ServerCallbacks serverCallbacks;
    
    private Action _updateMethod;

    public SteamTransportService()
    {
        serverCallbacks = new ServerCallbacks();
    }
    
    public void Update()
    {
        _updateMethod();
    }

    private void ServerUpdate()
    {
        serverSocket.Receive();
    }
    
    private void ClientUpdate()
    {
        clientConnection.Receive();
    }

    public bool CreateServer()
    {
        serverSocket = SteamNetworkingSockets.CreateRelaySocket<SocketManager>();
        
        if (serverSocket == null)
        {
            Logger.Error("Failed to create Steam relay server.");
            return false;
        }
        
        _updateMethod = ServerUpdate;
        TransportManager.Instance.ExecuteProcessMethodStatus(true);
        serverSocket.Interface = serverCallbacks;
        return true;
    }

    public bool ConnectToServer(string serverId)
    {
        ulong steamIdValue = ulong.Parse(serverId);
        SteamId hostSteamId = new SteamId { Value = steamIdValue };
        
        clientConnection = SteamNetworkingSockets.ConnectRelay<ClientConnectionManager>(hostSteamId, 0);
        
        if (clientConnection == null)
        {
            return false;
        }
        
        _updateMethod = ClientUpdate;
        TransportManager.Instance.ExecuteProcessMethodStatus(true);

        return true;
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
    
    public void CreateAndSendReliablePacketToServer(byte mainType, byte subType, byte playerIndex, byte[] data)
    {
        IntPtr packet = PacketFactory.CreateReliablePacket(mainType, subType, playerIndex, data, out int totalSize);
        clientConnection?.Connection.SendMessage(packet, totalSize);
        Marshal.FreeHGlobal(packet);
    }
    
    public void CreateAndSendUnreliablePacketToServer(byte mainType, byte subType, byte playerIndex, ushort tick, byte[] data)
    {
        IntPtr packetPtr = PacketFactory.CreateUnreliablePacket(mainType, subType, playerIndex, tick, data, out int totalSize);
        clientConnection?.Connection.SendMessage(packetPtr, totalSize, SendType.Unreliable);
        Marshal.FreeHGlobal(packetPtr);
    }

    public void CreateAndSendReliablePacketToClients(byte mainType, byte subType, byte playerIndex, byte[] data)
    {
        IntPtr packetPtr = PacketFactory.CreateReliablePacket(mainType, subType, playerIndex, data, out int totalSize);
        SendReliablePacketToClients(packetPtr, totalSize);
    }
    
    private void SendReliablePacketToClients(IntPtr packetPtr, int totalSize)
    {
        if (serverSocket == null) return;

        foreach (var client in serverSocket.Connected)
        {
            client.SendMessage(packetPtr, totalSize);
        }
        
        Marshal.FreeHGlobal(packetPtr);
    }

    public void CreateAndSendUnreliablePacketToClients(byte mainType, byte subType, byte playerIndex, ushort tick, byte[] data)
    {
        IntPtr packet = PacketFactory.CreateUnreliablePacket(mainType, subType, playerIndex, tick, data, out int totalSize);
        SendUnreliablePacketToClients(packet, totalSize);
    }
    
    private void SendUnreliablePacketToClients(IntPtr packetPtr, int totalSize)
    {
        if (serverSocket == null) return;

        foreach (var client in serverSocket.Connected)
        {
            client.SendMessage(packetPtr, totalSize, SendType.Unreliable);
        }
        
        Marshal.FreeHGlobal(packetPtr);
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

    public unsafe void OnMessage(Connection connection, NetIdentity identity, IntPtr data, int size, long messageNum, long recvTime, int channel)
    {
        var span = new Span<byte>((void*)data, size);

        byte mainType = span[0];
        byte subType = span[1];
        byte playerIndex = span[2];
        byte sendType = span[3];

        // Reliable
        if (sendType == 0) 
        {
            Span<byte> payload = span.Slice(PacketFactory.HeaderSizeReliable, size - PacketFactory.HeaderSizeReliable);
            TransportManager.Server.OnReliablePacketReceived(mainType, subType, playerIndex, payload);
        } 
        
        // Unreliable
        else
        { 
            ushort tick = BitConverter.ToUInt16(span.Slice(PacketFactory.HeaderSizeReliable, 2));
            Span<byte> payload = span.Slice(PacketFactory.HeaderSizeUnreliable, size - PacketFactory.HeaderSizeUnreliable);
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
        Logger.Network("Client: Successfully connected to server!");
    }

    public override void OnDisconnected(ConnectionInfo info)
    {
        Logger.Network("Client: Disconnected from server.");
        TransportManager.Instance.ExecuteProcessMethodStatus(false);
    }

    public override unsafe void OnMessage(IntPtr data, int size, long messageNum, long recvTime, int channel)
    {
        var span = new Span<byte>((void*)data, size);

        byte mainType = span[0];
        byte subType = span[1];
        byte playerIndex = span[2];
        byte sendType = span[3];

        // Reliable
        if (sendType == 0) 
        {
            Span<byte> payload = span.Slice(PacketFactory.HeaderSizeReliable, size - PacketFactory.HeaderSizeReliable);
            TransportManager.Client.OnReliablePacketReceived(mainType, subType, playerIndex, payload);
        } 
        
        // Unreliable
        else
        { 
            ushort tick = BitConverter.ToUInt16(span.Slice(PacketFactory.HeaderSizeReliable, 2));
            Span<byte> payload = span.Slice(PacketFactory.HeaderSizeUnreliable, size - PacketFactory.HeaderSizeUnreliable);
            TransportManager.Client.OnUnreliablePacketReceived(mainType, subType, playerIndex, tick, payload);
        }
    }
}