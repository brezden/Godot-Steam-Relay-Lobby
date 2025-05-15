using Godot;

public partial class MainMenuButtons : Node
{
    public override void _Ready()
    {
        var hostOnlineButton = GetNode<Button>("HostOnline");
        var exitButton = GetNode<Button>("ExitGame");

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
