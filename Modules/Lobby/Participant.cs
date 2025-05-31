using System;
using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Core.Lobby;

public partial class LobbyManager
{
    public static void PlayerReadyToJoinGame()
    {
        Logger.Lobby("Player is ready to join game");
        _lobbyService.EnterLobbyScene();
    }

    public static void ErrorJoiningLobby()
    {
        Logger.Error("Error joining lobby");
        SceneManager.Instance.ModalManager.RenderInformationModal(
            "[ERR-005] Failed to join lobby",
            InformationModalType.Error,
            "An error occurred while trying to join the lobby. Please try again");
        LeaveLobbyAndTransport();
    }
    
    public static void AddPlayer(string playerId)
    {
        if (LobbyMembersData.Players.ContainsKey(playerId))
        {
            Logger.Error($"Player {playerId} is already in the lobby");
            return;
        }

        var playerInfo = _lobbyService.GetPlayerInfo(playerId).Result;
        LobbyMembersData.Players.Add(playerId, playerInfo);
        Logger.Lobby($"Player added to lobby: {playerId}");
        EventBus.Lobby.OnLobbyMemberJoined(playerId);
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

    public static void LeaveLobbyAndTransport()
    {
        _lobbyService.LeaveLobby();
        _isHost = false;
        LobbyMembersData.Players.Clear();
        Logger.Lobby("Disconnected from lobby");
        
        TransportManager.Instance.Disconnect();
    }
}
