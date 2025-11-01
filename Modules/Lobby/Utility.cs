using System.Collections.Generic;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby;

public partial class LobbyManager 
{
    public static void GatherLobbyMembers()
    {
        LobbyMembersData = _lobbyService.GatherLobbyMembersData();
        Logger.Lobby($"Lobby members gathered: {LobbyMembersData.Players.Count}");
    }
    
    public static List<PlayerInvite> GetInGameFriends()
    {
        var PlayerList = _lobbyService.GetInGameFriends();
        return PlayerList;
    }

    public static void OpenInviteOverlay()
    {
        _lobbyService.OpenInviteOverlay();
        Logger.Lobby("Opened invite overlay");
    }
}
