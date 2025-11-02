using System.Collections.Generic;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby.MemberData;

public class MemberDataManager
{
    public Dictionary<ulong, PlayerInfo> Members = new Dictionary<ulong, PlayerInfo>();
    
    public PlayerInfo GetMember(ulong playerId)
    {
        return Members[playerId];
    }
    
    public void UpdateMember(PlayerInfo playerInfo)
    {
        Members[playerInfo.PlayerId] = playerInfo;
        EventBus.Lobby.OnLobbyDataUpdated();
    }
    
    public void RemoveMember(ulong playerId)
    {
        Members.Remove(playerId);
        EventBus.Lobby.OnLobbyDataUpdated();
    }
}