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
        public static event EventHandler LobbyCreated;
        public static event EventHandler LobbyJoined;
        public static event EventHandler LobbyDataUpdated; 
        public static event EventHandler<string> LobbyLog;

        public static void OnStartHost(ConnectionType connectionType)
        {
            StartHost?.Invoke(null, connectionType);
        }
        
        public static void OnStartGuest(ulong lobbyId)
        {
            StartGuest?.Invoke(null, lobbyId);
        }

        public static void OnLobbyMessageReceived(LobbyMessageArgs args)
        {
            LobbyMessageReceived?.Invoke(null,
                new LobbyMessageArgs { PlayerName = args.PlayerName, Message = args.Message });
        }
        
        public static void OnLobbyDataUpdated()
        {
            LobbyDataUpdated?.Invoke(null, EventArgs.Empty);
        }
        
        public static void OnLobbyLog(string message)
        {
            LobbyLog?.Invoke(null, message);
        }
        
        public static void OnLobbyCreated()
        {
            LobbyCreated?.Invoke(null, EventArgs.Empty);
        }

        public static void OnLobbyJoined()
        {
            LobbyJoined?.Invoke(null, EventArgs.Empty);
        }
    }
}
