using System;
using GodotPeer2PeerSteamCSharp.Core.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Scene;
using Steamworks;

namespace GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;

public partial class LobbyService : ILobbyService
{
    public void EnterLobbyScene()
    {
        SceneManager.Instance.GotoScene(SceneRegistry.Lobby.OnlineLobby);
    }
    
    public void InvitePlayer(string playerId)
    {
        try
        {
            _lobby.InviteFriend(ConvertStringToSteamId(playerId));
        }
        catch (Exception ex)
        {
            Logger.Error($"Error inviting player: {ex.Message}");
        }
    }

    public void LeaveLobby()
    {
        _lobby.Leave();
        _lobbyId = 0;
    }

    private void RegisterParticipantCallbacks()
    {
        SteamMatchmaking.OnLobbyEntered += OnLobbyEnteredCallback;
        SteamMatchmaking.OnLobbyMemberDisconnected += OnLobbyMemberLeft;
        SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeft;
    }

    private void OnLobbyEnteredCallback(Steamworks.Data.Lobby lobby)
    {
        _lobby = lobby;
        EventBus.Lobby.OnLobbyEntered(lobby.Owner.Id.ToString());
    }

    private static void OnLobbyMemberLeft(Steamworks.Data.Lobby lobby, Friend friend)
    {
        LobbyManager.RemovePlayer(friend.Id.ToString());
    }
}
