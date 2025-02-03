using Godot;
using System;

public partial class EventBus: Node
{
    public static event EventHandler<GlobalTypes.LobbyMessageArgs> LobbyMessageReceived;
    
    public static void OnLobbyMessageReceived(string playerName, string message)
    {
        LobbyMessageReceived?.Invoke(null, new GlobalTypes.LobbyMessageArgs { PlayerName = playerName, Message = message });
    }
}