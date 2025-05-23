using System;
using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Core.Lobby;

public partial class LobbyManager
{
    private void RegisterHostCallbacks()
    {
        EventBus.Lobby.CreateLobby += CreateLobby;
        EventBus.Lobby.LobbyCreated += OnLobbyCreated;
    }

    private async void CreateLobby(object? sender, EventArgs e)
    {
        try
        {
            SceneManager.Instance.ModalManager.RenderInformationModal(
                "Creating lobby",
                InformationModalType.Loading);

            _isHost = true;
            await _lobbyService.CreateLobby(4);
        }
        catch (Exception ex)
        {
            Logger.Error($"[ERR-001] Exception creating lobby: {ex.Message}");

            SceneManager.Instance.ModalManager.RenderInformationModal(
                "[ERR-001] Failed to create lobby",
                InformationModalType.Error,
                "An unexpected error occurred while creating the lobby. Please try again.");
            LeaveLobby();
        }
    }

    private void OnLobbyCreated(object? sender, string lobbyId)
    {
        Logger.Lobby($"Lobby created: {lobbyId}");
    }
}
