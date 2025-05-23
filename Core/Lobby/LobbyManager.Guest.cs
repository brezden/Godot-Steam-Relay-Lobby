using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Core.Lobby;

public partial class LobbyManager
{


    public static void AttemptingToJoinLobby(string lobbyId)
    {
        Logger.Lobby($"Attempting to join lobby: {lobbyId}");
        SceneManager.Instance.ModalManager.RenderInformationModal(
            "Joining lobby",
            InformationModalType.Loading);
    }
}
