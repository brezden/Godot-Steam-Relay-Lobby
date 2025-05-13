using Godot;
using System;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class MainMenuButtons : Node
{
    public override void _Ready()
    {
        Button hostOnlineButton = GetNode<Button>("HostOnline");
        Button exitButton = GetNode<Button>("ExitGame");

        hostOnlineButton.Pressed += HostOnline;
        exitButton.Pressed += ExitGame;
    }

    private void HostOnline()
    {
        EventBus.Lobby.OnCreateLobby();
    }

    private void ExitGame()
    {
        GetTree().Quit();
    }
}
