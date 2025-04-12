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
        serverSocket = SteamNetworkingSockets.CreateRelaySocket<SocketManager>(0);
        
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
        byte[] packet = PacketFactory.CreateUnreliablePacket(mainType, subType, playerIndex, tick, data);
        clientConnection?.Connection.SendMessage(packet, SendType.Unreliable, 1);
        PacketFactory.ReturnPacket(packet);
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
            client.SendMessage(packetPtr, totalSize, SendType.Unreliable, 1);
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

    public void OnMessage(Connection connection, NetIdentity identity, IntPtr data, int size, long messageNum,
        long recvTime, int channel)
    {
        byte[] packetData = new byte[size];
        System.Runtime.InteropServices.Marshal.Copy(data, packetData, 0, size);

        // var (header, payload) = PacketFactory.ParsePacket(packetData);
        //Logger.Network($"Server: Received packet - Main Type: {header.MainType}, Sub Type: {header.SubType} from Player {header.PlayerIndex}");
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

    public override void OnMessage(IntPtr data, int size, long messageNum, long recvTime, int channel)
    {
        Logger.Network("Server: Received message from server.");
        byte[] packetData = ArrayPool<byte>.Shared.Rent(size);

        try
        {
            Marshal.Copy(data, packetData, 0, size);
            TransportManager.Instance.OnClientPacketRecieved(packetData);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(packetData);
        }
    }
}