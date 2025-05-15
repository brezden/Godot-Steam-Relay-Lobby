using System;
using Godot;
using GodotPeer2PeerSteamCSharp.Core.Lobby;
using GodotPeer2PeerSteamCSharp.Routers;

namespace GodotPeer2PeerSteamCSharp.Core;

public partial class TransportManager : Node
{
    private ITransportService _transportService;

    public static TransportManager Instance
    {
        get;
        private set;
    }

    public override void _Ready()
    {
        Instance = this;

        _transportService = new TransportService();
        SetProcess(false);
    }

    public override void _Process(double delta)
    {
        _transportService.Update();
    }

    public bool IsConnectionActive()
    {
        return _transportService.IsConnectionActive();
    }

    public void ExecuteProcessMethodStatus(bool status)
    {
        SetProcess(status);
    }

    public void Disconnect()
    {
        _transportService.Disconnect();
        ExecuteProcessMethodStatus(false);
        Logger.Network("Disconnected from server.");
    }

    public class Server
    {
        public static void CreateServer()
        {
            Instance._transportService.CreateServer();
            Logger.Network("Server created.");
        }

        public static void SendReliablePacket(PacketTypes.MainType mainType, byte subType, byte playerIndex,
            Span<byte> data = default)
        {
            Instance._transportService.CreateAndSendReliablePacketToClients((byte) mainType, subType, playerIndex,
                data);
            Logger.Network(
                $"Sent reliable packet to clients. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}");
        }

        public static void SendUnreliablePacket(PacketTypes.MainType mainType, byte subType, byte playerIndex,
            ushort tick, Span<byte> data = default)
        {
            Instance._transportService.CreateAndSendUnreliablePacketToClients((byte) mainType, subType, playerIndex,
                tick, data);
            Logger.Network(
                $"Sent unreliable packet to clients. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}, Tick: {tick}");
        }

        public static void OnReliablePacketReceived(byte mainType, byte subType, byte playerIndex,
            Span<byte> data = default)
        {
            Logger.Network($"Received packet. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}");
        }

        public static void OnUnreliablePacketReceived(byte mainType, byte subType, byte playerIndex, ushort tick,
            Span<byte> data = default)
        {
            Logger.Network(
                $"Received packet. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}, Tick: {tick}");
        }
    }

    public class Client
    {
        public static void ConnectToServer(string serverId)
        {
            Logger.Network($"Attempting to connect to server: {serverId}");
            Instance._transportService.ConnectToServer(serverId);
        }

        public static void OnSuccessfulConnection()
        {
            Logger.Network("Successfully connected to server. (Client)");
            Instance._transportService.SetUpdateMethod("Client");
            Instance.ExecuteProcessMethodStatus(true);
            LobbyManager.LobbyConnectionGate.MarkTransportReady();
        }

        public static void SendReliablePacket(PacketTypes.MainType mainType, byte subType, byte playerIndex,
            Span<byte> data = default)
        {
            Instance._transportService.CreateAndSendReliablePacketToServer((byte) mainType, subType, playerIndex, data);
            Logger.Network(
                $"Sent reliable packet to server. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}");
        }

        public static void SendUnreliablePacket(PacketTypes.MainType mainType, byte subType, byte playerIndex,
            ushort tick, Span<byte> data = default)
        {
            Instance._transportService.CreateAndSendUnreliablePacketToServer((byte) mainType, subType, playerIndex,
                tick,
                data);
            Logger.Network(
                $"Sent unreliable packet to server. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}, Tick: {tick}");
        }

        public static void OnReliablePacketReceived(byte mainType, byte subType, byte playerIndex,
            Span<byte> data = default)
        {
            Dispatcher.ReliablePacketRouter(mainType, subType, playerIndex, data);
            Logger.Network($"Received packet. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}");
        }

        public static void OnUnreliablePacketReceived(byte mainType, byte subType, byte playerIndex, ushort tick,
            Span<byte> data = default)
        {
            Logger.Network(
                $"Received packet. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}, Tick: {tick}");
        }
    }

    public class Dispatcher
    {
        public static void ReliablePacketRouter(byte mainType, byte subType, byte playerIndex,
            Span<byte> data = default)
        {
            switch ((PacketTypes.MainType) mainType)
            {
                case PacketTypes.MainType.Scene:
                    SceneRouter.Route(subType, playerIndex, data);
                    break;
                default:
                    Logger.Error($"Unknown main type: {mainType}");
                    break;
            }
        }
    }
}
