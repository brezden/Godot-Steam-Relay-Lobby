using System;
using Steamworks;

namespace GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;

public partial class LobbyService : ILobbyService
{
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
        Logger.Network($"Joined lobby: {lobby.Id}");
        _lobby = lobby;
        LobbyManager.OnLobbyJoin(lobby.Owner.Id.ToString());
        LobbyManager.GatherLobbyMembers();
    }

    private static void OnLobbyMemberLeft(Steamworks.Data.Lobby lobby, Friend friend)
    {
        LobbyManager.OnRemovePlayer(friend.Id.ToString());
    }
}
