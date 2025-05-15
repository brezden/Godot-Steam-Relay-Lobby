using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class LobbyManager : Node
{
    // Lobby Data
    public static LobbyMembersData _lobbyMembersData = new();
    private static bool _isHost;

    // Readiness Tracker
    public static ReadinessTracker _readiness = new();
    private ILobbyService LobbyService;

    public LobbyManager Instance
    {
        get;
        private set;
    }

    public override void _Ready()
    {
        Instance = this;

        LobbyService = new LobbyService();
        LobbyService.Initialize();
        EventBus.Lobby.CreateLobby += CreateLobby;
    }

    public override void _Process(double delta)
    {
        LobbyService.Update();
    }

    private static async void CreateLobby(object? sender, EventArgs e)
    {
        try
        {
            SceneManager.Instance.ModalManager.RenderInformationModal(
                "Creating lobby",
                InformationModalType.Loading);

            await Instance.LobbyService.CreateLobby(4);
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

            Instance.LobbyService.LeaveLobby();
            TransportManager.Instance.Disconnect();
        }
    }

    public static void GatherLobbyMembers()
    {
        try
        {
            _lobbyMembersData = Instance.LobbyService.GatherLobbyMembersData().Result;
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
            Instance.LobbyService.LeaveLobby();
            TransportManager.Instance.Disconnect();
        }
    }

    public static void OnPlayerReadyToJoinGame()
    {
        Logger.Network("Player is ready to join game");
        _readiness = new ReadinessTracker();
        SceneManager.Instance.GotoScene(SceneRegistry.Lobby.OnlineLobby);
    }

    public static void AttemptingToJoinLobby(string lobbyId)
    {
        Logger.Network($"Attempting to join lobby: {lobbyId}");
        SceneManager.Instance.ModalManager.RenderInformationModal(
            "Joining lobby",
            InformationModalType.Loading);
    }

    public static void OnLobbyJoin(string lobbyId)
    {
        if (_isHost)
            return;

        Logger.Network($"Joining lobby: {lobbyId}");
        _readiness.MarkLobbyEntered();

        try
        {
            TransportManager.Client.ConnectToServer(lobbyId);
        }
        catch
        {
            Logger.Error($"[ERR-006] Failed to attempt connection to socket: {lobbyId}");
            SceneManager.Instance.ModalManager.RenderInformationModal(
                "[ERR-006] Failed to connect to socket",
                InformationModalType.Error,
                "An error occurred while connecting to the lobby. Please try again.");
            Instance.LobbyService.LeaveLobby();
        }
    }

    public static void SendLobbyMessage(string message)
    {
        Instance.LobbyService.SendLobbyMessage(message);
        Logger.Network($"Lobby message sent: {message}");
    }

    public static void OnLobbyMessageReceived(string sender, string message)
    {
        var args = new GlobalTypes.LobbyMessageArgs
        {
            PlayerName = sender,
            Message = message
        };

        Logger.Network($"Lobby message received from {sender}: {message}");

        EventBus.Lobby.OnLobbyMessageReceived(sender, message);
    }

    public static void OnPlayerAdded(string playerId)
    {
        var playerInfo = Instance.LobbyService.GetPlayerInfo(playerId).Result;
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
        Instance.LobbyService.LeaveLobby();
        TransportManager.Instance.Disconnect();
        _isHost = false;
        _lobbyMembersData.Players.Clear();
    }

    public static void InvitePlayer(string playerId)
    {
        Instance.LobbyService.InvitePlayer(playerId);
        Logger.Network($"Player invited: {playerId}");
    }

    public static Task<List<GlobalTypes.PlayerInvite>> GetInGameFriends()
    {
        try
        {
            return Instance.LobbyService.GetInGameFriends();
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
    private bool LobbyInformationGathered;
    private bool TransportReady;

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
            LobbyManager.OnPlayerReadyToJoinGame();
    }
}
