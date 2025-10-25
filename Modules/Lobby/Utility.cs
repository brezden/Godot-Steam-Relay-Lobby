using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GodotPeer2PeerSteamCSharp.Types.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby;

public partial class LobbyManager 
{
    public static void GatherLobbyMembers()
    {
        LobbyMembersData = _lobbyService.GatherLobbyMembersData();
        Logger.Lobby($"Lobby members gathered: {LobbyMembersData.Players.Count}");
    }
    
    public static Task<List<PlayerInvite>> GetInGameFriends()
    {
        try
        {
            return _lobbyService.GetInGameFriends();
        }
        catch (Exception e)
        {
            Logger.Error($"Failed to get online friends. {e.Message}");
            return Task.FromResult(new List<PlayerInvite>());
        }
    }
}
