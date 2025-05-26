using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Core.Lobby;

public partial class LobbyManager 
{
    public static async Task GatherLobbyMembers()
    {
        LobbyMembersData = _lobbyService.GatherLobbyMembersData().Result;
        Logger.Lobby($"Lobby members gathered: {LobbyMembersData.Players.Count}");
    }
    
    public static Task<List<GlobalTypes.PlayerInvite>> GetInGameFriends()
    {
        try
        {
            return _lobbyService.GetInGameFriends();
        }
        catch (Exception e)
        {
            Logger.Error($"Failed to get online friends. {e.Message}");
            return Task.FromResult(new List<GlobalTypes.PlayerInvite>());
        }
    }
}
