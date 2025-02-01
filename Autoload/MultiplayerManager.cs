using System;
using System.Collections.Generic;
using Godot;
using Steamworks;

public partial class MultiplayerManager : Node
{
    public static MultiplayerManager Instance { get; private set; }
    private IMultiplayerService _multiplayerService;
    
    public static Dictionary<string, GlobalTypes.PlayerInfo> Players = new Dictionary<string, GlobalTypes.PlayerInfo>();
    
    public static event EventHandler<PlayerInformationArgs> PlayerJoinedLobby;
    
    public class PlayerInformationArgs : EventArgs
    {
        public ImageTexture PlayerPicture { get; set; }
        public string PlayerName { get; set; }
    }

    public override void _Ready()
    {
        Instance = this;

        _multiplayerService = new SteamMultiplayerService();
        _multiplayerService.Initialize();
    }
    
    public override void _Process(double delta)
	{
        _multiplayerService.Update();
	}

    public static void CreateLobby()
    {
        Instance._multiplayerService.CreateLobby(4);
    }

    public static void InviteLobbyOverlay()
    {
        Instance._multiplayerService.InviteLobbyOverlay();
    }

    public static void MemberJoinLobby(Image playerPicture, string playerName, string playerID)
    {
        GD.Print("Player joined lobby: " + playerName);
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