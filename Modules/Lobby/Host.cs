using System;
using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby;

public partial class LobbyManager
{
    private void RegisterHostCallbacks()
    {
        EventBus.Lobby.StartHost += (_, _) => StartHost();
    }

    private static async void StartHost()
    {
        try
        {
            UIManager.Instance.ModalManager.RenderInformationModal(
                "Creating lobby",
                InformationModalType.Loading);
            await _lobbyService.StartHost();
        }
        catch (Exception ex)
        {
            Logger.Error($"[ERR-001] Exception creating lobby: {ex.Message}");
            UIManager.Instance.ModalManager.RenderInformationModal(
                "Failed to create lobby",
                InformationModalType.Error,
                "An unexpected error occurred while creating the lobby. Please try again",
                001);
            LeaveLobbyAndTransport();
        }
    }
}
