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

    public static void InvitePlayer(ulong playerId)
    {
        _lobbyService.InvitePlayer(playerId);
        Logger.Lobby($"Player invited: {playerId}");
    }

    public static void LeaveLobbyAndTransport()
    {
        _lobbyService.LeaveLobby();
        MemberData = null;
        Logger.Lobby("Disconnected from lobby");
    }
}
