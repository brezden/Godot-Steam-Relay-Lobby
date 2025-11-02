using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class LobbyButtons : Node
{
    Button startGameButton;
    Button inviteLobbyButton;
    Button leaveLobbyButton;
    
    public override void _Ready()
    {
        startGameButton = GetNode<Button>("%StartGame");
        inviteLobbyButton = GetNode<Button>("%InviteMembers");
        leaveLobbyButton = GetNode<Button>("%LeaveLobby");

        inviteLobbyButton.Pressed += InviteLobby;
        leaveLobbyButton.Pressed += LeaveLobby;
        startGameButton.Pressed += StartGame;
    }

    private void InviteLobby()
    {
        LobbyManager.OpenInviteOverlay();
    }

    private void LeaveLobby()
    {
        LobbyManager.LeaveLobbyAndTransport();
        SceneManager.Instance.GotoScene(SceneRegistry.MainMenu.Home);
    }

    private void StartGame()
    {
        Logger.Game("Game is starting...");   
    }
}
