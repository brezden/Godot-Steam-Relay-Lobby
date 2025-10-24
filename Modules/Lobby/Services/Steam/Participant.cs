using System;
using GodotSteam;
using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby.Services;

public partial class LobbyService
{
    private Steam.LobbyJoinedEventHandler _onLobbyJoinedHandler;
    
    private void RegisterParticipantCallbacks()
    {
        _onLobbyJoinedHandler += OnLobbyJoined;
        Steam.LobbyJoined += _onLobbyJoinedHandler;
        
        SteamMatchmaking.OnLobbyEntered += OnLobbyEnteredCallback;
        SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoinedCallback;
        SteamMatchmaking.OnLobbyMemberDisconnected += OnLobbyMemberLeft;
        SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeft;
    }
    
    public void EnterLobbyScene()
    {
        SceneManager.Instance.GotoScene(SceneRegistry.Lobby.OnlineLobby);
    }
    
    public void InvitePlayer(ulong playerId)
    {
        try
        {
            Steam.InviteUserToLobby(_lobbyId, playerId);
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



    private static void OnLobbyJoined(ulong lobby, long permissions, bool locked, long response)
    {
        Logger.Lobby($"Joined lobby: {lobby} with response code: {response}");
    }

    private void OnLobbyMemberJoinedCallback(Steamworks.Data.Lobby lobby, Friend friend)
    {
        LobbyManager.AddPlayer(friend.Id.ToString());
    }

    private static void OnLobbyMemberLeft(Steamworks.Data.Lobby lobby, Friend friend)
    {
        LobbyManager.RemovePlayer(friend.Id.ToString());
    }
}
