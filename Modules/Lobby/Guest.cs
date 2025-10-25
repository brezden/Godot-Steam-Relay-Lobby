using System;
using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby;

public partial class LobbyManager
{
    private void RegisterGuestCallbacks()
    {
        EventBus.Lobby.StartGuest += (_, lobbyId) => StartGuest(lobbyId);
    }
    
    // TODO: Hook up
    private static void StartGuest(ulong lobbyId)
    {
        try {
            UIManager.Instance.ModalManager.RenderInformationModal(
                "Joining lobby",
                InformationModalType.Loading);
            _lobbyService.JoinLobby(lobbyId);
        } catch (Exception ex)
        {
            ErrorGuest(ex);
        }
    }

    public static void ErrorGuest(Exception ex)
    {
        Logger.Error($"[ERR-002] Exception joining lobby: {ex.Message}");
        UIManager.Instance.ModalManager.RenderInformationModal(
            "Failed to join lobby",
            InformationModalType.Error,
            "An unexpected error occurred while joining the lobby. Please try again",
            002);
        LeaveLobbyAndTransport();
    }
}
