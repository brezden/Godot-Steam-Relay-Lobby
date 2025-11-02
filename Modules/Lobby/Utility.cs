using System.Collections.Generic;
using GodotPeer2PeerSteamCSharp.Modules.Lobby.MemberData;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby;

public partial class LobbyManager 
{
    public static void InitializeLobbyData()
    {
        MemberData = new MemberDataManager();
        
        LobbyMembersData initialData = _lobbyService.GatherLobbyMembersData();
        
        foreach (var player in initialData.Players.Values)
        {
            MemberData.UpdateMember(player);
        }
        
        Logger.Lobby($"Lobby members gathered: {initialData.Players.Count}");
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
