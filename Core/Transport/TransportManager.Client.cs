using System;
using System.Runtime.InteropServices.ComTypes;
using GodotPeer2PeerSteamCSharp.Core.Lobby;

namespace GodotPeer2PeerSteamCSharp.Core;

public partial class TransportManager
{
    public static class Client
    {
        public static void RegisterCallbacks()
        {
            EventBus.Lobby.LobbyEntered += (_,_) => ConnectToServer();
        }
        
        public static void ConnectToServer()
        {
            if (LobbyManager.Instance.isPlayerHost()) return;
            
            try
            {
                var serverId = LobbyManager.Instance.GetServerId();
                Instance._transportService.ConnectToServer(serverId);
            }
            catch (Exception e)
            {
                // TODO
                Logger.Error("oops");
            }
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
            Dispatcher.ReliablePacket(mainType, subType, playerIndex, data);
            Logger.Network($"Received packet. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}");
        }

        public static void OnUnreliablePacketReceived(byte mainType, byte subType, byte playerIndex, ushort tick,
            Span<byte> data = default)
        {
            Logger.Network(
                $"Received packet. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}, Tick: {tick}");
        }
    }
}
