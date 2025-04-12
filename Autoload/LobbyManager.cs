using System;
using System.Collections.Generic;
using Godot;
using Steamworks;

public partial class LobbyManager : Node
{
    public static LobbyManager Instance { get; private set; }
    private ILobbyService _lobbyService;
    
    public static Dictionary<string, GlobalTypes.PlayerInfo> Players = new Dictionary<string, GlobalTypes.PlayerInfo>();
    private static bool _isHost = false;

    public override void _Ready()
    {
        Instance = this;

        _lobbyService = new SteamLobbyService();
        _lobbyService.Initialize();
    }
    
    public override void _Process(double delta)
	{
        _lobbyService.Update();
	}

    public static void CreateLobby()
    {
        Instance._lobbyService.CreateLobby(4);
    }

    public static void OnLobbyCreation(string lobbyId)
    {
        Logger.Network($"Lobby created: {lobbyId}");
        bool result = TransportManager.Server.CreateServer();
        
        if (!result)
        {
            Logger.Error("Failed to create server.");
            return;
        }
        
        _isHost = true;
    }
    
    public static void OnLobbyJoin(string lobbyId)
    {
        if (_isHost) return;
        
        bool result = TransportManager.Client.ConnectToServer(lobbyId);

        if (!result)
        {
            Logger.Error($"Failed to connect to lobby and socket successfully: {lobbyId}");
            return;
        } 
        
        Logger.Network($"Successfully connected to lobby and socket: {lobbyId}");
    }
    
    public static void SendLobbyMessage(string message)
    {
        Instance._lobbyService.SendLobbyMessage(message);
        Logger.Network($"Lobby message sent: {message}");
    }
    
    public static void OnLobbyMessageReceived(string sender, string message)
    {
        GlobalTypes.LobbyMessageArgs args = new GlobalTypes.LobbyMessageArgs
        {
            PlayerName = sender,
            Message = message
        };
        
        Logger.Network($"Lobby message received from {sender}: {message}");
        
        EventBus.Lobby.OnLobbyMessageReceived(sender, message);
    }
    
    public static void InviteLobbyOverlay()
    {
        Instance._lobbyService.InviteLobbyOverlay();
    }
    
    public static void AddPlayer(ImageTexture playerPicture, string playerName, SteamId playerId)
    {
        Players.Add(playerId.ToString(), new GlobalTypes.PlayerInfo
        {
            PlayerId = playerId.ToString(),
            Name = playerName,
            ProfilePicture = playerPicture,
            IsReady = false
        });
        
        EventBus.Lobby.OnLobbyMemberJoined(playerId.ToString());
        Logger.Network($"Player added: {playerName}");
    }

    public static void RemovePlayer(string playerId)
    {
        Players.Remove(playerId);
        EventBus.Lobby.OnLobbyMemberLeft(playerId);
        Logger.Network($"Player removed: {playerId}");
    }
    
    public static void LeaveLobby()
    {
        Instance._lobbyService.LeaveLobby();
        TransportManager.Instance.ExecuteProcessMethodStatus(false);
        TransportManager.Client.Disconnect();
        _isHost = false;
        Players.Clear();
        SceneManager.Instance.GotoScene("res://main.tscn");
        Logger.Network("Left lobby and cleared player list.");
    }
}