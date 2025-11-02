using System.Runtime.CompilerServices;
using GodotPeer2PeerSteamCSharp.Types.Lobby;
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
        bool userInvited = Steam.InviteUserToLobby(_lobbyId, playerId);
        
        // Not sure under what conditions this would fail, but logging it just in case.
        if (!userInvited)
        {
            Logger.Error($"Failed to invite player {playerId} to lobby {_lobbyId}");
        }
    }

    public void LeaveLobby()
    {
        Steam.LeaveLobby(_lobbyId);
        _lobbyId = 0;
    }

    // For some reason the lobby member joined and left events are not part of the C#
    // bindings for GodotSteam. They are passed in through chat updates.
    private void OnLobbyChatUpdate( 
        ulong lobbyId, 
        long changedId, 
        long makingChangeId, 
        long chatState)
    {
        string name;
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
                name = GetSteamNameById((ulong)changedId) ?? "Unknown";
                Logger.Lobby($"Player has left the lobby: {name} ({changedId})", true);
                OnLobbyMemberLeft((ulong) changedId);
                break;
            case CHAT_MEMBER_STATE_CHANGE_DISCONNECTED:
                name = GetSteamNameById((ulong)changedId) ?? "Unknown";
                Logger.Lobby($"Player has disconnected from the lobby: {name} ({changedId})", true);
                OnLobbyMemberLeft((ulong) changedId);
                break;
            case CHAT_MEMBER_STATE_CHANGE_KICKED:
                name = GetSteamNameById((ulong)changedId) ?? "Unknown";
                Logger.Lobby($"Player has been kicked from the lobby: {name} ({changedId})", true);
                OnLobbyMemberLeft((ulong) changedId);
                break;
            case CHAT_MEMBER_STATE_CHANGE_BANNED:
                name = GetSteamNameById((ulong)changedId) ?? "Unknown";
                Logger.Lobby($"Player has been banned from the lobby: {name} ({changedId})", true);
                OnLobbyMemberLeft((ulong) changedId);
                break;
        }
    }

    private static void OnLobbyJoined(ulong lobby, long permissions, bool locked, long response)
    {
        long CHAT_ROOM_ENTER_RESPONSE_SUCCESS = 1;
        
        if (response != CHAT_ROOM_ENTER_RESPONSE_SUCCESS)
        {
            Logger.Error($"Failed to join lobby {lobby}. Response code: {response}");
            return;
        }
        
        _lobbyId = lobby;
        Logger.Lobby($"Joined lobby {_lobbyId}", true);
        LobbyManager.InitializeLobbyData();
        LobbyManager.PlayerReadyToJoinGame();
    }

    private void OnLobbyMemberJoinedCallback(ulong memberJoinedId)
    {
        Logger.Lobby($"Player has joined the lobby: {memberJoinedId}", true);
        PlayerInfo newMemberData = GetLobbyMember(memberJoinedId);
        LobbyManager.MemberData.UpdateMember(newMemberData);
    }

    private static void OnLobbyMemberLeft(ulong memberLeftId)
    {
        Logger.Lobby($"Player has left the lobby: {memberLeftId}", true);
        LobbyManager.MemberData.RemoveMember(memberLeftId);
    }
}
