using System;
using Godot;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

public partial class EventBus : Node
{
    public static class Lobby
    {
        public static event EventHandler<ConnectionType> StartHost;
        public static event EventHandler<ulong> StartGuest;
        public static event EventHandler<LobbyMessageArgs> LobbyMessageReceived;
        public static event EventHandler<ulong> LobbyMemberJoined;
        public static event EventHandler<ulong> LobbyMemberLeft;
        public static event EventHandler LobbyMembersRefreshed;
        public static event EventHandler<string> LobbyLog;

        public static void OnStartHost(ConnectionType connectionType)
        {
            StartHost?.Invoke(null, connectionType);
        }
        
        public static void OnStartGuest(ulong lobbyId)
        {
            StartGuest?.Invoke(null, lobbyId);
        }

        public static void OnLobbyMemberJoined(ulong playerId)
        {
            LobbyMemberJoined?.Invoke(null, playerId);
        }

        public static void OnLobbyMemberLeft(ulong playerId)
        {
            LobbyMemberLeft?.Invoke(null, playerId);
        }

        public static void OnLobbyMessageReceived(LobbyMessageArgs args)
        {
            LobbyMessageReceived?.Invoke(null,
                new LobbyMessageArgs { PlayerName = args.PlayerName, Message = args.Message });
        }
        
        // TODO: Hook this up
        public static void OnLobbyMembersRefreshed()
        {
            LobbyMembersRefreshed?.Invoke(null, null);
        }
        
        public static void OnLobbyLog(string message)
        {
            LobbyLog?.Invoke(null, message);
        }
    }
}
