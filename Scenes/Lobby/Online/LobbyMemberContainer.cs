using Godot;
using GodotPeer2PeerSteamCSharp.Core.Lobby;

public partial class LobbyMemberContainer : Node
{
    [Export] private PackedScene _lobbyMemberScene;

    public override void _Ready()
    {
        EventBus.Lobby.LobbyMemberJoined += (_, playerId) => AddLobbyMember(playerId);
        EventBus.Lobby.LobbyMemberLeft += (_,playerId) => RemoveLobbyMember(playerId);

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

    private void AddLobbyMember(string playerId)
    {
        var lobbyMemberInstance = _lobbyMemberScene.Instantiate();
        var args = LobbyManager.LobbyMembersData.Players[playerId];

        lobbyMemberInstance.Name = args.PlayerId;
        
        if (lobbyMemberInstance is LobbyMember lobbyMemberScript)
        {
            lobbyMemberScript.PlayerName = args.Name;
            lobbyMemberScript.ProfilePicture = args.ProfilePicture;
        }

        AddChild(lobbyMemberInstance);
    }

    private void RemoveLobbyMember(string playerId)
    {
        var lobbyMemberInstance = GetNodeOrNull<LobbyMember>(playerId);
        if (lobbyMemberInstance != null)
            lobbyMemberInstance.QueueFree();
        else
            Logger.Error($"Lobby member with ID {playerId} not found");
    }
}
