using System;
using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby;

public partial class LobbyManager
{
    private void RegisterGuestCallbacks()
    {
        EventBus.Lobby.StartGuest += (_, lobbyId) => StartGuest(lobbyId);
    }
    
    private static async void StartGuest(string lobbyId)
    {
        try {
            UIManager.Instance.ModalManager.RenderInformationModal(
                "Joining lobby",
                InformationModalType.Loading);
            await _lobbyService.StartGuest(lobbyId);
        } catch (Exception ex)
        {
            ErrorGuest(ex);
        }
    }

    public static void ErrorGuest(Exception ex)
    {
        Logger.Error($"[ERR-002] Exception joining lobby: {ex.Message}");
        UIManager.Instance.ModalManager.RenderInformationModal(
            "[ERR-002] Failed to join lobby",
            InformationModalType.Error,
            "An unexpected error occurred while joining the lobby. Please try again");
        LeaveLobbyAndTransport();
    }

    public static void FinishGuest()
    {
        _lobbyService.EnterLobbyScene();
    }
    
    public string GetServerId()
    {
        string serverId = _lobbyService.GetServerId();
        Logger.Lobby($"Retrieved server ID: {serverId}");
        return serverId;
    }
}
