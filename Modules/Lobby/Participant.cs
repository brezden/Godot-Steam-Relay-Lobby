using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby;

public partial class LobbyManager
{
    public static void PlayerReadyToJoinGame()
    {
        Logger.Lobby("Player is ready to join game");
        SceneManager.Instance.GotoScene(SceneRegistry.Lobby.OnlineLobby);
    }

    public static void ErrorJoiningLobby()
    {
        Logger.Error("Error joining lobby");
        UIManager.Instance.ModalManager.RenderInformationModal(
            "Failed to join lobby",
            InformationModalType.Error,
            "An error occurred while trying to join the lobby. Please try again",
            005);
        LeaveLobbyAndTransport();
    }
    
    public static void AddPlayer(ulong playerId)
    {
        if (LobbyMembersData.Players.ContainsKey(playerId))
        {
            Logger.Error($"Player {playerId} is already in the lobby");
            return;
        }

        var playerInfo = _lobbyService.GetLobbyMember(playerId);
        LobbyMembersData.Players.Add(playerId, playerInfo);
        Logger.Lobby($"Player added to lobby: {playerId}");
        EventBus.Lobby.OnLobbyMemberJoined(playerId);
    }

    public static void RemovePlayer(ulong playerId)
    {
        LobbyMembersData.Players.Remove(playerId);
        Logger.Lobby($"Player removed from lobby: {playerId}");
        EventBus.Lobby.OnLobbyMemberLeft(playerId);
    }

    public static void InvitePlayer(ulong playerId)
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
    }
}
