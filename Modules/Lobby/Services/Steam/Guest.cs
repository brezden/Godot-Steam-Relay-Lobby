using GodotSteam;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby.Services;

public partial class LobbyService 
{
    public void JoinLobby(ulong lobbyId)
    {
        Steam.JoinLobby(lobbyId);
    }
}