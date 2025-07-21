using System;
using GodotPeer2PeerSteamCSharp.Modules.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Scene;
using Steamworks;

namespace GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;

public partial class LobbyService
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
        if (_lobby.Id != 0)
        {
            _lobby.Leave();
        }

        _lobby = default;
        _lobbyId = 0;
    }

    private void RegisterParticipantCallbacks()
    {
        SteamMatchmaking.OnLobbyEntered += OnLobbyEnteredCallback;
        SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoinedCallback;
        SteamMatchmaking.OnLobbyMemberDisconnected += OnLobbyMemberLeft;
        SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeft;
    }

    private void OnLobbyEnteredCallback(Steamworks.Data.Lobby lobby)
    {
        _lobby = lobby;
    }

    private void OnLobbyMemberJoinedCallback(Steamworks.Data.Lobby lobby, Friend friend)
    {
        LobbyManager.AddPlayer(friend.Id.ToString());
    }

    private static void OnLobbyMemberLeft(Steamworks.Data.Lobby lobby, Friend friend)
    {
        LobbyManager.RemovePlayer(friend.Id.ToString());
    }

    public string GetServerId()
    {
        uint ip = 0;
        ushort port = 0;
        SteamId serverId = default;

        bool hasServer = _lobby.GetGameServer(ref ip, ref port, ref serverId);

        if (hasServer && serverId != 0)
        {
            return serverId.ToString();
        }
        
        throw new Exception("Unable to get server ID");
    }
}
