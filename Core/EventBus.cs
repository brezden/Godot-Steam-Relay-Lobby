using Godot;
using System;

public partial class EventBus: Node
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
        public static event EventHandler CreateLobby;
        public static event EventHandler<GlobalTypes.LobbyMessageArgs> LobbyMessageReceived;
        public static event EventHandler<string> LobbyMemberJoined;
        public static event EventHandler<string> LobbyMemberLeft;

        public static void OnCreateLobby()
        {
            CreateLobby?.Invoke(null, EventArgs.Empty);
        }
        
        public static void OnLobbyMemberJoined(string playerId)
        {
            LobbyMemberJoined?.Invoke(null, playerId);
        }
        
        public static void OnLobbyMemberLeft(string playerId)
        {
            LobbyMemberLeft?.Invoke(null, playerId);
        }
        
        public static void OnLobbyMessageReceived(string playerId, string message)
        {
            LobbyMessageReceived?.Invoke(null, new GlobalTypes.LobbyMessageArgs { PlayerName = playerId, Message = message });
        }
    }
}