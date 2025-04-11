using System;
using Godot;
using Steamworks;
using Steamworks.Data;

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
    
    public void SendReliablePacketToServer(PacketTypes.MainType mainType, byte subType, byte playerIndex, byte[] data)
    {
        byte[] packet = PacketFactory.CreateReliablePacket((byte) mainType, subType, playerIndex, data);
        clientConnection?.Connection.SendMessage(packet);
        PacketFactory.ReturnPacket(packet);
    }
    
    public void SendUnreliablePacketToServer(PacketTypes.MainType mainType, byte subType, byte playerIndex, ushort tick, byte[] data)
    {
        byte[] packet = PacketFactory.CreateUnreliablePacket((byte) mainType, subType, playerIndex, tick, data);
        clientConnection?.Connection.SendMessage(packet, SendType.Unreliable);
        PacketFactory.ReturnPacket(packet);
    }
    
    public void SendReliablePacketToClients(PacketTypes.MainType mainType, byte subType, byte playerIndex, byte[] data)
    {
        byte[] packet = PacketFactory.CreateReliablePacket((byte) mainType, subType, playerIndex, data);

        if (serverSocket != null)
        {
            foreach (var client in serverSocket.Connected)
            {
                client.SendMessage(packet);
            }
        }
        
        PacketFactory.ReturnPacket(packet);
    }

    public void SendUnreliablePacketToClients(PacketTypes.MainType mainType, byte subType, byte playerIndex, ushort tick, byte[] data)
    {
        byte[] packet = PacketFactory.CreateUnreliablePacket((byte)mainType, subType, playerIndex, tick, data);

        if (serverSocket != null)
        {
            foreach (var client in serverSocket.Connected)
            {
                client.SendMessage(packet, SendType.Unreliable);
            }
        }

        PacketFactory.ReturnPacket(packet);
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
        byte[] packetData = new byte[size];
        System.Runtime.InteropServices.Marshal.Copy(data, packetData, 0, size);
        // var (header, payload) = PacketFactory.ParsePacket(packetData);
    }
}