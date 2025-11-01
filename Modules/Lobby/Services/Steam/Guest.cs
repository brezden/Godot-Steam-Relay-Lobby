using GodotSteam;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby.Services;

public partial class LobbyService 
{
    private Steam.JoinRequestedEventHandler _joinRequested;
    
    public void RegisterGuestCallbacks()
    {
        _joinRequested += OnJoinRequested;
        Steam.JoinRequested += _joinRequested;
    }
    
    public void JoinLobby(ulong lobbyId)
    {
        Steam.JoinLobby(lobbyId);
    }
    
    private void OnJoinRequested(ulong lobbyId, ulong steamId)
    {
        JoinLobby(lobbyId); 
    }
}