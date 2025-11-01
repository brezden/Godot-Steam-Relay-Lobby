using System;
using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

public partial class LobbyChat : Node
{
    [Export] public bool AutoScrollToBottom = true;
    
    private TextEdit chatBox;
    private LineEdit chatInput;

    public override void _Ready()
    {
        chatBox = GetNode<TextEdit>("%ChatBox");
        chatInput = GetNode<LineEdit>("%ChatInput");

        EventBus.Lobby.LobbyMessageReceived += OnLobbyMessageReceived;

        var sendLobbyMessageButton = GetNode<Button>("%SendLobbyMessage");
        sendLobbyMessageButton.Pressed += OnSendLobbyMessage;
    }

    private void OnSendLobbyMessage()
    {
        if (string.IsNullOrEmpty(chatInput.Text))
            return;

        LobbyManager.SendLobbyMessage(chatInput.Text);
        chatInput.Text = string.Empty;
    }

    public void OnLobbyMessageReceived(object sender, LobbyMessageArgs e)
    {
        chatBox.Text += e.PlayerName + ": " + e.Message + "\n";

        if (AutoScrollToBottom)
        {
           int last = Math.Max(chatBox.GetLineCount() - 1, 0);
           chatBox.SetCaretLine(last, false, false);
           chatBox.SetCaretColumn(chatBox.GetLine(last).Length);
        }
    }
}
