using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using GodotPeer2PeerSteamCSharp.Core.Lobby.Gates;
using GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Scene;

namespace GodotPeer2PeerSteamCSharp.Core.Lobby;

public partial class LobbyManager : Node
{
    public static LobbyMembersData LobbyMembersData = new();
    public static LobbyConnectionGate LobbyConnectionGate = new();

    private static ILobbyService _lobbyService;
    private static bool _isHost;

    public static LobbyManager Instance
    {
        get;
        private set;
    }

    public override void _Ready()
    {
        Instance = this;

        _lobbyService = new LobbyService();
        _lobbyService.Initialize();

        RegisterHostCallbacks();
    }

    public override void _Process(double delta)
    {
        _lobbyService.Update();
    }

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
