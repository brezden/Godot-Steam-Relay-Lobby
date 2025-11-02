using System.Collections.Generic;
using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Lobby;

public partial class LobbyMemberContainer : Node
{
    [Export] private PackedScene _lobbyMemberScene;
    private int lobbyMemberCount = 0;

    public override void _Ready()
    {
        EventBus.Lobby.LobbyMembersRefreshed += (_, _) => RefreshLobbyData();
        
        if (_lobbyMemberScene == null)
        {
            Logger.Error("LobbyMember scene is not assigned in the inspector");
            return;
        }

        if (LobbyManager.LobbyMembersData.Players.Count == 0)
        {
            Logger.Error("No players found in the lobby data");
            return;
        }

        foreach (var playerId in LobbyManager.LobbyMembersData.Players.Keys)
            AddLobbyMember(playerId);
    }

    private void RefreshLobbyData()
    {
        var currentPlayerIds = LobbyManager.LobbyMembersData.Players.Keys;
        var existingPlayerIds = new HashSet<ulong>();

        foreach (var child in GetChildren())
        {
            if (child is LobbyMember lobbyMember)
            {
                if (ulong.TryParse(lobbyMember.Name, out ulong playerId))
                {
                    existingPlayerIds.Add(playerId);
                }
            }
        }

        foreach (var playerId in currentPlayerIds)
        {
            if (!existingPlayerIds.Contains(playerId))
            {
                AddLobbyMember(playerId);
            }
        }

        foreach (var playerId in existingPlayerIds)
        {
            if (existingPlayerIds.Contains(playerId))
            {
                RemoveLobbyMember(playerId);
            }
        }
    }

    private void AddLobbyMember(ulong playerId)
    {
        
        var lobbyMemberInstance = _lobbyMemberScene.Instantiate();
        var args = LobbyManager.LobbyMembersData.Players[playerId];

        lobbyMemberInstance.Name = playerId.ToString();
        var lobbyMemberContainer = lobbyMemberInstance.GetNodeOrNull<LobbyMember>("%LobbyMemberContainer");
        
        if (lobbyMemberContainer is LobbyMember lobbyMemberScript)
        {
            lobbyMemberScript.PlayerName = args.Name;

            if (args.ProfilePicture != null)
            {
                lobbyMemberScript.ProfilePicture = args.ProfilePicture;
            }
        }

        AddChild(lobbyMemberInstance);
        lobbyMemberCount++;
    }

    private void RemoveLobbyMember(ulong playerId)
    {
        var lobbyMemberInstance = GetNodeOrNull<LobbyMember>(playerId.ToString());
        if (lobbyMemberInstance != null)
        {
            lobbyMemberInstance.QueueFree();
            lobbyMemberCount--;
        }
        else
            Logger.Error($"Lobby member with ID {playerId} not found");
    }
}
