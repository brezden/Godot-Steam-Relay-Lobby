using System;
using GodotPeer2PeerSteamCSharp.Core.Lobby;
using GodotPeer2PeerSteamCSharp.Core.Lobby.Gates;
using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Core;

public partial class TransportManager
{
    public static class Server
    {
        public static void RegisterCallbacks()
        {
            EventBus.Lobby.LobbyCreated += (_, _) => CreateServer();
        }
        
        private static void CreateServer()
        {
            try
            {
                Instance._transportService.CreateServer();
                Logger.Network("Server created.");
                LobbyManager.LobbyConnectionGate.MarkTransportReady();
            } 
            catch (Exception ex)
            {
                Logger.Error($"[ERR-002] Failed to create transport server: {ex}");

                SceneManager.Instance.ModalManager.RenderInformationModal(
                    "[ERR-002] Failed to create server",
                    InformationModalType.Error,
                    "An error occurred while creating the transport server for the lobby. Please try again.");

                LobbyManager.LeaveLobby();
            }
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
}
