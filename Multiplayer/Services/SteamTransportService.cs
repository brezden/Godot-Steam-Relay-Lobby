using System;
using Godot;
using Steamworks;
using Steamworks.Data;

public class SteamTransportService : ITransportService
{
    private SocketManager serverSocket;
    private ClientConnectionManager clientConnection;
    private ServerCallbacks serverCallbacks;

    public SteamTransportService()
    {
        serverCallbacks = new ServerCallbacks();
    }

    public void CreateServer()
    {
        if (!SteamClient.IsValid)
        {
            GD.PrintErr("Steam Client is not initialized!");
            return;
        }

        GD.Print("Creating Steam relay server...");

        serverSocket = SteamNetworkingSockets.CreateRelaySocket<SocketManager>(0);
        
        if (serverSocket == null)
        {
            GD.PrintErr("Failed to create Steam relay server.");
            return;
        }

        serverSocket.Interface = serverCallbacks;

        GD.Print("Server is now listening for connections...");
    }

    public void ConnectToServer(SteamId hostSteamId)
    {
        if (!SteamClient.IsValid)
        {
            GD.PrintErr("Steam Client is not initialized!");
            return;
        }

        GD.Print($"Connecting to server hosted by Steam ID: {hostSteamId}");

        clientConnection = SteamNetworkingSockets.ConnectRelay<ClientConnectionManager>(hostSteamId, 0);

        if (clientConnection == null)
        {
            GD.PrintErr("Failed to connect to server.");
            return;
        }

        GD.Print("Successfully started client connection.");
    }
}

public class ServerCallbacks : ISocketManager
{
    public void OnConnecting(Connection connection, ConnectionInfo data)
    {
        connection.Accept();
        GD.Print($"Server: Player {data.Identity} is attempting to connect...");
    }

    public void OnConnected(Connection connection, ConnectionInfo data)
    {
        GD.Print($"Server: Player {data.Identity} has successfully connected!");
    }

    public void OnDisconnected(Connection connection, ConnectionInfo data)
    {
        GD.Print($"Server: Player {data.Identity} has disconnected.");
    }

    public void OnMessage(Connection connection, NetIdentity identity, IntPtr data, int size, long messageNum, long recvTime, int channel)
    {
        GD.Print($"Server: Received message from {identity}!");
    }
}

public class ClientConnectionManager : ConnectionManager
{
    public override void OnConnecting(ConnectionInfo info)
    {
        GD.Print("Client: Attempting to connect to server...");
    }

    public override void OnConnected(ConnectionInfo info)
    {
        GD.Print("Client: Successfully connected to server!");
    }

    public override void OnDisconnected(ConnectionInfo info)
    {
        GD.Print("Client: Disconnected from server.");
    }

    public override void OnMessage(IntPtr data, int size, long messageNum, long recvTime, int channel)
    {
        GD.Print("Client: Received message from server!");
    }
}