using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using GodotPeer2PeerSteamCSharp.Autoload.Types.Scene;
using GodotPeer2PeerSteamCSharp.Multiplayer.Types;
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
        EventBus.Lobby.CreateLobby += CreateLobby;
    }
    
    public override void _Process(double delta)
	{
        _lobbyService.Update();
	}

    private static async void CreateLobby(object? sender, EventArgs e)
    {
        try
        {
            SceneManager.Instance.ModalManager.RenderInformationModal(
                "Creating lobby",
                InformationModalType.Loading);

            await Instance._lobbyService.CreateLobby(4);
        }
        catch (Exception ex)
        {
            Logger.Error($"[ERR-001] Exception creating lobby: {ex.Message}");

            SceneManager.Instance.ModalManager.RenderInformationModal(
                "[ERR-001] Failed to create lobby",
                InformationModalType.Error,
                "An unexpected error occurred while creating the lobby. Please try again.");
        }
    }

    public static void OnLobbyCreation(string lobbyId)
    {
        Logger.Network($"Lobby created: {lobbyId}");
        _isHost = true;

        try
        {
            TransportManager.Server.CreateServer();
        }
        catch (Exception ex)
        {
            Logger.Error($"[ERR-002] Failed to create transport server: {ex}");

            SceneManager.Instance.ModalManager.RenderInformationModal(
                "[ERR-002] Failed to create server",
                InformationModalType.Error,
                "An error occurred while creating the transport server for the lobby. Please try again.");
            
            Instance._lobbyService.LeaveLobby();
            TransportManager.Instance.ExecuteProcessMethodStatus(false);
            return;
        }

        SceneManager.Instance.GotoScene(SceneRegistry.Lobby.OnlineLobby);
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
        SceneManager.Instance.GotoScene(SceneRegistry.MainMenu.Home);
    }
    
    public static void InvitePlayer(string playerId)
    {
        Instance._lobbyService.InvitePlayer(playerId);
        Logger.Network($"Player invited: {playerId}");
    }
    
    public static Task<List<GlobalTypes.PlayerInvite>> GetInGameFriends()
    {
        try
        {
            return Instance._lobbyService.GetInGameFriends();
        }
        catch (Exception e)
        {
            Logger.Error($"Failed to get online friends. {e.Message}");
            return Task.FromResult(new List<GlobalTypes.PlayerInvite>());
        }
    }
}