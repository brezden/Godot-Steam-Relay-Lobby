using System;
using Godot;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

public partial class EventBus : Node
{
    public static class UI
    {
        public static event EventHandler CloseModal;

        public static void OnCloseModal()
        {
            CloseModal?.Invoke(null, EventArgs.Empty);
        }
    }

    public static class Lobby
    {
        public static event EventHandler<ConnectionType> StartHost;
        public static event EventHandler CreateLobby;
        public static event EventHandler<string> LobbyCreated;
        public static event EventHandler<string> LobbyEntered;
        public static event EventHandler<GlobalTypes.LobbyMessageArgs> LobbyMessageReceived;
        public static event EventHandler<string> LobbyMemberJoined;
        public static event EventHandler<string> LobbyMemberLeft;

        public static void OnStartHost(ConnectionType connectionType)
        {
            StartHost?.Invoke(null, connectionType);
        }
        
        public static void OnCreateLobby()
        {
            CreateLobby?.Invoke(null, EventArgs.Empty);
        }

        public static void OnLobbyCreated(string lobbyId)
        {
            LobbyCreated?.Invoke(null, lobbyId);
        }

        public static void OnLobbyEntered(string lobbyId)
        {
            LobbyEntered?.Invoke(null, lobbyId); 
        }

        public static void OnLobbyMemberJoined(string playerId)
        {
            LobbyMemberJoined?.Invoke(null, playerId);
        }

        public static void OnLobbyMemberLeft(string playerId)
        {
            LobbyMemberLeft?.Invoke(null, playerId);
        }

        public static void OnLobbyMessageReceived(LobbyMessageArgs args)
        {
            LobbyMessageReceived?.Invoke(null,
                new GlobalTypes.LobbyMessageArgs { PlayerName = args.PlayerName, Message = args.Message });
        }
    }
}
