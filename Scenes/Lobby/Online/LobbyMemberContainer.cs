using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

public partial class LobbyMemberContainer : Node
{
    [Export] private PackedScene _lobbyMemberScene;

    public override void _Ready()
    {
        if (_lobbyMemberScene == null)
        {
            Logger.Error("LobbyMember scene is not assigned in the inspector");
            return;
        }

        EventBus.Lobby.LobbyDataUpdated += OnLobbyDataUpdated;

        // Initial render
        RefreshLobbyData();
    }

    public override void _ExitTree()
    {
        EventBus.Lobby.LobbyDataUpdated -= OnLobbyDataUpdated;
    }

    private void OnLobbyDataUpdated(object sender, EventArgs args)
    {
        RefreshLobbyData();
    }

    private void RefreshLobbyData()
    {
        if (LobbyManager.MemberData == null || LobbyManager.MemberData.Members == null)
        {
            Logger.Error("MemberData or Members is null; nothing to refresh.");
            return;
        }

        // Desired set of players (authoritative source)
        var desiredIds = LobbyManager.MemberData.Members.Keys;

        // Current set of players in the UI (from child node names)
        var currentIds = new HashSet<ulong>();
        foreach (Node child in GetChildren())
        {
            if (ulong.TryParse(child.Name, out var id))
                currentIds.Add(id);
        }

        // Add or update desired members
        foreach (var playerId in desiredIds)
            UpdateOrCreateLobbyMember(playerId);

        // Remove any members not in desired set
        foreach (var staleId in currentIds)
        {
            if (!desiredIds.Contains(staleId))
                RemoveLobbyMember(staleId);
        }
    }

    /// <summary>
    /// Update if exists, otherwise create.
    /// </summary>
    private void UpdateOrCreateLobbyMember(ulong playerId)
    {
        var lobbyMember = GetLobbyMemberById(playerId);
        if (lobbyMember == null)
        {
            CreateLobbyMember(playerId);
        }
        else
        {
            ApplyMemberData(lobbyMember, LobbyManager.MemberData.Members[playerId]);
            // R
        }
    }

    /// <summary>
    /// Creates a new lobby member UI entry.
    /// Assumes the root of _lobbyMemberScene has LobbyMember script. If not, tries to find one below.
    /// </summary>
    private void CreateLobbyMember(ulong playerId)
    {
        if (_lobbyMemberScene == null)
        {
            Logger.Error("Cannot create lobby member: _lobbyMemberScene is null.");
            return;
        }

        var instance = _lobbyMemberScene.Instantiate();
        instance.Name = playerId.ToString();

        LobbyMember lobbyMemberScript = instance as LobbyMember;

        lobbyMemberScript ??= FindFirstLobbyMemberBelow(instance);

        if (lobbyMemberScript == null)
        {
            Logger.Error("Packed scene does not contain a LobbyMember script on root or children.");
            AddChild(instance);
            return;
        }

        if (LobbyManager.MemberData.Members.TryGetValue(playerId, out var info))
            ApplyMemberData(lobbyMemberScript, info);

        AddChild(instance);
    }

    /// <summary>
    /// Removes the UI entry for a member.
    /// </summary>
    private void RemoveLobbyMember(ulong playerId)
    {
        var node = GetNodeOrNull<Node>(playerId.ToString());
        if (node != null)
        {
            node.QueueFree();
        }
        else
        {
            Logger.Error($"Lobby member with ID {playerId} not found for removal.");
        }
    }

    /// <summary>
    /// Looks up the LobbyMember instance for a given player ID.
    /// </summary>
    private LobbyMember GetLobbyMemberById(ulong playerId)
    {
        // We name each member container node with the playerId string.
        var containerNode = GetNodeOrNull<Node>(playerId.ToString());
        if (containerNode == null) return null;

        // If the root is LobbyMember, return it.
        if (containerNode is LobbyMember lmRoot)
            return lmRoot;

        // Otherwise search its descendants for a LobbyMember script.
        return FindFirstLobbyMemberBelow(containerNode);
    }

    /// <summary>
    /// Depth-first search for the first LobbyMember component below a node.
    /// </summary>
    private LobbyMember FindFirstLobbyMemberBelow(Node root)
    {
        if (root is LobbyMember lm) return lm;

        foreach (Node child in root.GetChildren())
        {
            var found = FindFirstLobbyMemberBelow(child);
            if (found != null)
                return found;
        }

        return null;
    }

    /// <summary>
    /// Applies PlayerInfo to a LobbyMember UI component.
    /// </summary>
    private void ApplyMemberData(LobbyMember lobbyMember, PlayerInfo info)
    {
        lobbyMember.PlayerName = info.Name;

        if (info.ProfilePicture != null)
            lobbyMember.ProfilePicture = info.ProfilePicture;
    }
}
