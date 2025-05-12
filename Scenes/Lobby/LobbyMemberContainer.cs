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
        EventBus.Lobby.LobbyMemberJoined += OnLobbyMemberJoined;
        EventBus.Lobby.LobbyMemberLeft += OnLobbyMemberLeft;

        //

        // UPON THIS BEING CREATED FILL IN ALL THE PLAYERS THAT ARE ALREADY IN THE LOBBY FROM THE LobbyManager.Players

        //
    }

    private void OnLobbyMemberJoined(object sender, string playerId)
    {
        if (_lobbyMemberScene == null)
        {
            GD.PushError("LobbyMember scene is null! Make sure to assign it in the Inspector.");
            return;
        }

        var lobbyMemberInstance = _lobbyMemberScene.Instantiate();

        GlobalTypes.PlayerInfo args = LobbyManager.Players[playerId];

        lobbyMemberInstance.Name = args.PlayerId;

        if (lobbyMemberInstance is LobbyMember lobbyMemberScript)
        {
            lobbyMemberScript.PlayerName = args.Name;
            lobbyMemberScript.ProfilePicture = args.ProfilePicture;
        }

        AddChild(lobbyMemberInstance);
    }

    private void OnLobbyMemberLeft(object sender, string playerId)
    {
        foreach (LobbyMember child in GetChildren())
        {
            if (child.Name == playerId)
            {
                RemoveChild(child);
                child.QueueFree();
                break;
            }
        }
    }
}
