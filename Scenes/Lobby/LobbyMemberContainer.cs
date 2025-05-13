using Godot;
using System;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

public partial class LobbyMemberContainer : Node
{
    [Export] private PackedScene _lobbyMemberScene;

    public override void _Ready()
    {
        if (_lobbyMemberScene == null)
        {
            Logger.Error("LobbyMember scene is not assigned in the inspector.");
            return;
        }

        if (LobbyManager._lobbyMembersData.Players.Count == 0)
        {
            Logger.Error("No players found in the lobby data.");
            return;
        }

        foreach (var playerId in LobbyManager._lobbyMembersData.Players.Keys)
        {
            AddLobbyMember(playerId);
        }
    }

    private void AddLobbyMember(string playerId)
    {

        var lobbyMemberInstance = _lobbyMemberScene.Instantiate();
        PlayerInfo args = LobbyManager._lobbyMembersData.Players[playerId];

        lobbyMemberInstance.Name = args.PlayerId;

        if (lobbyMemberInstance is LobbyMember lobbyMemberScript)
        {
            lobbyMemberScript.PlayerName = args.Name;
            lobbyMemberScript.ProfilePicture = args.ProfilePicture;
        }

        AddChild(lobbyMemberInstance);
    }
}
