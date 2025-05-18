using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Core.Lobby;

public partial class LobbyManager 
{
    public static void GatherLobbyMembers()
    {
        try
        {
            LobbyMembersData = _lobbyService.GatherLobbyMembersData().Result;
            Logger.Network($"Lobby members gathered: {LobbyMembersData.Players.Count}");
            LobbyConnectionGate.MarkLobbyInformationGathered();
        }
        catch (Exception ex)
        {
            Logger.Error($"[ERR-003] Failed to get lobby members: {ex.Message}");
            SceneManager.Instance.ModalManager.RenderInformationModal(
                "[ERR-003] Failed to gather lobby members",
                InformationModalType.Error,
                "An error occurred while gathering lobby members. Please try again.");
            _lobbyService.LeaveLobby();
            TransportManager.Instance.Disconnect();
        }
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
