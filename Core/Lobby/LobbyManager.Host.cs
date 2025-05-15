using System;
using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Core.Lobby;

public partial class LobbyManager
{
    private void RegisterHostCallbacks()
    {
        EventBus.Lobby.CreateLobby += CreateLobby;
    }

    private async void CreateLobby(object? sender, EventArgs e)
    {
        try
        {
            SceneManager.Instance.ModalManager.RenderInformationModal(
                "Creating lobby",
                InformationModalType.Loading);

            await _lobbyService.CreateLobby(4);
            LobbyConnectionGate.MarkLobbyEntered();
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

    public void OnLobbyCreation(string lobbyId)
    {
        Logger.Network($"Lobby created: {lobbyId}");
        _isHost = true;

        try
        {
            TransportManager.Server.CreateServer();
            LobbyConnectionGate.MarkTransportReady();
        }
        catch (Exception ex)
        {
            Logger.Error($"[ERR-002] Failed to create transport server: {ex}");

            SceneManager.Instance.ModalManager.RenderInformationModal(
                "[ERR-002] Failed to create server",
                InformationModalType.Error,
                "An error occurred while creating the transport server for the lobby. Please try again.");

            _lobbyService.LeaveLobby();
            TransportManager.Instance.Disconnect();
        }
    }
}
