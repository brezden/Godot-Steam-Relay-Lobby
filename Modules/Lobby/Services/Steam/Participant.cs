using System;
using GodotSteam;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby.Services;

public partial class LobbyService
{
    private Steam.LobbyJoinedEventHandler _onLobbyJoinedHandler;
    private Steam.LobbyChatUpdateEventHandler _onLobbyChatUpdateHandler;
    
    private void RegisterParticipantCallbacks()
    {
        _onLobbyJoinedHandler += OnLobbyJoined;
        Steam.LobbyJoined += _onLobbyJoinedHandler;
        
        _onLobbyChatUpdateHandler +=  OnLobbyChatUpdate;
        Steam.LobbyChatUpdate += _onLobbyChatUpdateHandler;
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
        Steam.LeaveLobby(_lobbyId);
        _lobbyId = 0;
    }

    // For some reason the lobby member joined and left events are not part of the C#
    // bindings for GodotSteam. They are passed in through chat updates which is whats
    // being used here.
    private void OnLobbyChatUpdate( 
        ulong lobbyId, 
        long changedId, 
        long makingChangeId, 
        long chatState)
    {
        const int CHAT_MEMBER_STATE_CHANGE_ENTERED = 0x0001;
        const int CHAT_MEMBER_STATE_CHANGE_LEFT = 0x0002;
        const int CHAT_MEMBER_STATE_CHANGE_DISCONNECTED = 0x0004;
        const int CHAT_MEMBER_STATE_CHANGE_KICKED = 0x0008;
        const int CHAT_MEMBER_STATE_CHANGE_BANNED = 0x0010;
        
        switch(chatState)
        {
            case CHAT_MEMBER_STATE_CHANGE_ENTERED:
                OnLobbyMemberJoinedCallback((ulong) changedId);
                break;
            
            case CHAT_MEMBER_STATE_CHANGE_LEFT:
            case CHAT_MEMBER_STATE_CHANGE_DISCONNECTED:
            case CHAT_MEMBER_STATE_CHANGE_KICKED:
            case CHAT_MEMBER_STATE_CHANGE_BANNED:
                OnLobbyMemberLeft((ulong) changedId);
                break;
        }
    }

    private static void OnLobbyJoined(ulong lobby, long permissions, bool locked, long response)
    {
        LobbyManager.PlayerReadyToJoinGame();
    }

    private void OnLobbyMemberJoinedCallback(ulong memberJoinedId)
    {
        LobbyManager.AddPlayer(memberJoinedId);
    }

    private static void OnLobbyMemberLeft(ulong memberLeftId)
    {
        LobbyManager.RemovePlayer(memberLeftId);
    }
}
