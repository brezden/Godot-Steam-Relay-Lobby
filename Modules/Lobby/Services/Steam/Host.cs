using System.Threading.Tasks;
using GodotPeer2PeerSteamCSharp.Types.Lobby;
using GodotSteam;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby.Services;

public partial class LobbyService
{
    private void RegisterHostCallbacks()
    {
        Steam.LobbyCreated += (_,_) => EventBus.Lobby.OnLobbyCreated();
    }
    
    public async Task StartHost()
    {
        CreateLobby();
    }
    
    private void CreateLobby(LobbyType lobbyType = LobbyType.Private, int maxMembers = 4)
    {
        var steamType = lobbyType switch
        {
            LobbyType.Private     => Steam.LobbyType.Private,
            LobbyType.FriendsOnly => Steam.LobbyType.FriendsOnly,
            LobbyType.Public      => Steam.LobbyType.Public,
            _                     => Steam.LobbyType.Private
        };
        Steam.CreateLobby(steamType, maxMembers);
    }
}
