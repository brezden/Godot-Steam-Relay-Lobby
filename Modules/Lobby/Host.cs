using System;
using GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Scene;
using Steamworks;

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
            _lobbyService.EnterLobbyScene();
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

    public void SetServerId(string serverId)
    {
        _lobbyService.SetServerId(serverId);
        Logger.Lobby($"ServerId has been set: {serverId}");
    }
    
    public string GetServerId()
    {
        string serverId = _lobbyService.GetServerId();
        Logger.Lobby($"Retrieved server ID: {serverId}");
        return serverId;
    }
}
