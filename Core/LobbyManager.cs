using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using GodotPeer2PeerSteamCSharp.Types.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Scene;
using Steamworks;

public partial class LobbyManager : Node
{
    // Singleton instance
    private static LobbyManager Instance { get; set; }
    private ILobbyService _lobbyService;

    // Lobby Data
    public static LobbyMembersData _lobbyMembersData = new LobbyMembersData();
    private static bool _isHost = false;

    // Readiness Tracker
    private static ReadinessTracker _readiness = new ReadinessTracker();

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
            _readiness.MarkLobbyEntered();
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
            _readiness.MarkTransportReady();
        }
        catch (Exception ex)
        {
            Logger.Error($"[ERR-002] Failed to create transport server: {ex}");

            SceneManager.Instance.ModalManager.RenderInformationModal(
                "[ERR-002] Failed to create server",
                InformationModalType.Error,
                "An error occurred while creating the transport server for the lobby. Please try again.");

            Instance._lobbyService.LeaveLobby();
            TransportManager.Instance.Disconnect();
        }
    }

    public static void GatherLobbyMembers()
    {
        try
        {
            _lobbyMembersData = Instance._lobbyService.GatherLobbyMembersData().Result;
            Logger.Network($"Lobby members gathered: {_lobbyMembersData.Players.Count}");
            _readiness.MarkLobbyInformationGathered();
        }
        catch (Exception ex)
        {
            Logger.Error($"[ERR-003] Failed to get lobby members: {ex.Message}");
            SceneManager.Instance.ModalManager.RenderInformationModal(
                "[ERR-003] Failed to gather lobby members",
                InformationModalType.Error,
                "An error occurred while gathering lobby members. Please try again.");
            Instance._lobbyService.LeaveLobby();
            TransportManager.Instance.Disconnect();
        }
    }

    public static void OnPlayerReadyToJoinGame()
    {
        Logger.Network("Player is ready to join game");
        _readiness = new ReadinessTracker();
        SceneManager.Instance.GotoScene(SceneRegistry.Lobby.OnlineLobby);
    }

    public static void OnLobbyJoin(string lobbyId)
    {
        if (_isHost)
        {
            return;
        }

        Logger.Network($"Joining lobby: {lobbyId}");
        _readiness.MarkLobbyEntered();

        try
        {
            TransportManager.Client.ConnectToServer(lobbyId);
            Logger.Network($"Connected to socket: {lobbyId}");
            _readiness.MarkTransportReady();
        }
        catch
        {
            Logger.Error($"[ERR-006] Failed to connect to socket: {lobbyId}");
            SceneManager.Instance.ModalManager.RenderInformationModal(
                "[ERR-006] Failed to connect to socket",
                InformationModalType.Error,
                "An error occurred while connecting to the lobby. Please try again.");
            Instance._lobbyService.LeaveLobby();
        }
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

    public static void OnPlayerAdded(string playerId)
    {
        PlayerInfo playerInfo = Instance._lobbyService.GetPlayerInfo(playerId).Result;
        _lobbyMembersData.Players.Add(playerId, playerInfo);
        Logger.Network($"Player added to lobby: {playerId}");
        EventBus.Lobby.OnLobbyMemberJoined(playerId);
    }

    public static void OnRemovePlayer(string playerId)
    {
        _lobbyMembersData.Players.Remove(playerId);
        Logger.Network($"Player removed from lobby: {playerId}");
        EventBus.Lobby.OnLobbyMemberLeft(playerId);
    }

    public static void ErrorJoiningLobby()
    {
        Logger.Error("Error joining lobby");
        SceneManager.Instance.ModalManager.RenderInformationModal(
            "[ERR-005] Failed to join lobby",
            InformationModalType.Error,
            "An error occurred while trying to join the lobby. Please try again.");
        LeaveLobby();
    }

    public static void LeaveLobby()
    {
        Instance._lobbyService.LeaveLobby();
        TransportManager.Instance.Disconnect();
        _isHost = false;
        _lobbyMembersData.Players.Clear();
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

public class ReadinessTracker
{
    private bool LobbyEntered;
    private bool TransportReady;
    private bool LobbyInformationGathered;

    private bool IsReady => LobbyEntered && TransportReady && LobbyInformationGathered;

    public void MarkLobbyEntered()
    {
        LobbyEntered = true;
        CheckLobbyReady();
    }

    public void MarkTransportReady()
    {
        TransportReady = true;
        CheckLobbyReady();
    }

    public void MarkLobbyInformationGathered()
    {
        LobbyInformationGathered = true;
        CheckLobbyReady();
    }

    private void CheckLobbyReady()
    {
        if (IsReady)
        {
            LobbyManager.OnPlayerReadyToJoinGame();
        }
    }
}
