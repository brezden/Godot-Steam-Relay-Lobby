using System;
using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Core.Lobby;

public partial class LobbyManager
{
    public static void RegisterParticipantCallbacks()
    {
        EventBus.Lobby.LobbyEntered += OnLobbyEntered;
    }

    public static void OnLobbyEntered(object? sender, string lobbyId)
    {
        Logger.Lobby($"Joined lobby: {lobbyId}");
        LobbyConnectionGate.MarkLobbyEntered();
    }

    public bool isPlayerHost()
    {
        return _isHost;
    }
    
    public static void PlayerReadyToJoinGame()
    {
        Logger.Lobby("Player is ready to join game");
        _lobbyService.EnterLobbyScene();
    }

    public static void PlayerAdded(string playerId)
    {
        var playerInfo = _lobbyService.GetPlayerInfo(playerId).Result;
        LobbyMembersData.Players.Add(playerId, playerInfo);
        Logger.Lobby($"Player added to lobby: {playerId}");
        EventBus.Lobby.OnLobbyMemberJoined(playerId);
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

    public static void RemovePlayer(string playerId)
    {
        LobbyMembersData.Players.Remove(playerId);
        Logger.Lobby($"Player removed from lobby: {playerId}");
        EventBus.Lobby.OnLobbyMemberLeft(playerId);
    }

    public static void InvitePlayer(string playerId)
    {
        _lobbyService.InvitePlayer(playerId);
        Logger.Lobby($"Player invited: {playerId}");
    }

    public static void LeaveLobby()
    {
        _lobbyService.LeaveLobby();
        TransportManager.Instance.Disconnect();
        _isHost = false;
        LobbyMembersData.Players.Clear();
    }
}
