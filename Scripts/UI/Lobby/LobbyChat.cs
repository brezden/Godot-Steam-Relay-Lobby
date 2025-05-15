using Godot;
using GodotPeer2PeerSteamCSharp.Core.Lobby;

public partial class LobbyChat : Node
{
    private TextEdit chatBox;
    private LineEdit chatInput;

    public override void _Ready()
    {
        chatBox = GetNode<TextEdit>("ChatBox");
        chatInput = GetNode<LineEdit>("ChatInput");

        EventBus.Lobby.LobbyMessageReceived += OnLobbyMessageReceived;

        var sendLobbyMessageButton = GetNode<Button>("SendLobbyMessage");
        sendLobbyMessageButton.Pressed += OnSendLobbyMessage;
    }

    private void OnSendLobbyMessage()
    {
        if (string.IsNullOrEmpty(chatInput.Text))
            return;

        LobbyManager.SendLobbyMessage(chatInput.Text);
        chatInput.Text = string.Empty;
    }

    public void OnLobbyMessageReceived(object sender, GlobalTypes.LobbyMessageArgs e)
    {
        chatBox.Text += e.PlayerName + ": " + e.Message + "\n";
    }
}
