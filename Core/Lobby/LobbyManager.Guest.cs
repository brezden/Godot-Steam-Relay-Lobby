using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Core.Lobby;

public partial class LobbyManager
{
    public static void OnLobbyJoin(string lobbyId)
    {
        if (_isHost) return;

        Logger.Network($"Joining lobby: {lobbyId}");
        LobbyConnectionGate.MarkLobbyEntered();

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
            LeaveLobby();
        }

        GatherLobbyMembers();
    }

    public static void AttemptingToJoinLobby(string lobbyId)
    {
        Logger.Network($"Attempting to join lobby: {lobbyId}");
        SceneManager.Instance.ModalManager.RenderInformationModal(
            "Joining lobby",
            InformationModalType.Loading);
    }
}
