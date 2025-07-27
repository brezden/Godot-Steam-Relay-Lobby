using Godot;
using GodotPeer2PeerSteamCSharp.Types.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class MainMenuButtons : Node
{
    public override void _Ready()
    {
        var hostOnlineButton = GetNode<Button>("HostOnline");
        var settingsButton = GetNode<Button>("Settings");
        var exitButton = GetNode<Button>("ExitGame");

        hostOnlineButton.Pressed += HostOnline;
        settingsButton.Pressed += ShowSettings;
        exitButton.Pressed += ExitGame;
    }

    private void HostOnline()
    {
        EventBus.Lobby.OnStartHost(ConnectionType.Online);
    }
    
    private void ShowSettings()
    {
        UIManager.Instance.ModalManager.ShowModal(ModalType.Settings);
    }

    private void ExitGame()
    {
        GetTree().Quit();
    }
}
