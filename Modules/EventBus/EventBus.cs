using System;
using Godot;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

public partial class EventBus : Node
{
    public static class Lobby
    {
        public static event EventHandler<ConnectionType> StartHost;
        public static event EventHandler<string> StartGuest;
        public static event EventHandler<LobbyMessageArgs> LobbyMessageReceived;
        public static event EventHandler<string> LobbyMemberJoined;
        public static event EventHandler<string> LobbyMemberLeft;

        public static void OnStartHost(ConnectionType connectionType)
        {
            StartHost?.Invoke(null, connectionType);
        }
        
        public static void OnStartGuest(string lobbyId)
        {
            StartGuest?.Invoke(null, lobbyId);
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
                new LobbyMessageArgs { PlayerName = args.PlayerName, Message = args.Message });
        }
    }
}
