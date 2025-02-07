using System;
using System.Collections.Generic;
using Godot;
using Steamworks;

public partial class LobbyManager : Node
{
    public static LobbyManager Instance { get; private set; }
    private ILobbyService _lobbyService;
    
    private static bool _isHost = false;
    private static string _lobbyId = string.Empty;
    
    private static Dictionary<string, GlobalTypes.PlayerInfo> Players = new Dictionary<string, GlobalTypes.PlayerInfo>();
    
    public static event EventHandler<PlayerInformationArgs> PlayerJoinedLobby;
    
    public class PlayerInformationArgs : EventArgs
    {
        public ImageTexture PlayerPicture { get; set; }
        public string PlayerName { get; set; }
    }

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
        GD.Print("[LobbyManager] Lobby created: " + lobbyId);
        _isHost = true;
        _lobbyId = lobbyId;
        TransportManager.CreateServer();
    }
    
    public static void OnLobbyJoin(string lobbyId)
    {
        if (!_isHost)
        {
            TransportManager.ConnectToServer(lobbyId);
        }
    }
    
    public static void SendLobbyMessage(string message)
    {
        Instance._lobbyService.SendLobbyMessage(message);
    }
    
    public static void OnLobbyMessageReceived(string sender, string message)
    {
        GlobalTypes.LobbyMessageArgs args = new GlobalTypes.LobbyMessageArgs
        {
            PlayerName = sender,
            Message = message
        };
        
        EventBus.OnLobbyMessageReceived(sender, message);
    }
    
    public static void InviteLobbyOverlay()
    {
        Instance._lobbyService.InviteLobbyOverlay();
    }
    
    public void OnPlayerJoinedLobby(ImageTexture playerPicture, string playerName, SteamId playerId)
    {
        PlayerInformationArgs args = new PlayerInformationArgs
        {
            PlayerPicture = playerPicture,
            PlayerName = playerName,
        };
        
        Players.Add(playerId.ToString(), new GlobalTypes.PlayerInfo
        {
            Index = Players.Count,
            Name = playerName,
            ProfilePicture = playerPicture,
            SteamId = playerId,
            IsReady = false
        });
        
        PlayerJoinedLobby?.Invoke(this, args);
    }
}