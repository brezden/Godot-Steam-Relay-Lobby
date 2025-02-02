using Godot;
using System;

public partial class LobbyMemberContainer : Node
{
    [Export] private PackedScene _lobbyMemberScene;

    public override void _Ready()
    {
        if (_lobbyMemberScene == null)
        {
            GD.PushError("LobbyMember scene is not assigned!");
            return;
        }

        LobbyManager.PlayerJoinedLobby += OnPlayerJoinedLobby;
    }

    private void OnPlayerJoinedLobby(object sender, LobbyManager.PlayerInformationArgs args)
    {
        if (_lobbyMemberScene == null)
        {
            GD.PushError("LobbyMember scene is null! Make sure to assign it in the Inspector.");
            return;
        }

        var lobbyMemberInstance = _lobbyMemberScene.Instantiate();

        if (lobbyMemberInstance is LobbyMember lobbyMemberScript)
        {
            lobbyMemberScript.PlayerName = args.PlayerName;
            lobbyMemberScript.ProfilePicture = args.PlayerPicture;
        }

        AddChild(lobbyMemberInstance);
    }
}