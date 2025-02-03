using Godot;
using System;

public partial class LobbyChat : Node
{
    private TextEdit chatBox;
    private LineEdit chatInput;
    
    public override void _Ready()
    {
        chatBox = GetNode<TextEdit>("ChatBox");
        chatInput = GetNode<LineEdit>("ChatInput");

        EventBus.LobbyMessageReceived += OnLobbyMessageReceived;

        Button sendLobbyMessageButton = GetNode<Button>("SendLobbyMessage");
        sendLobbyMessageButton.Pressed += OnSendLobbyMessage;
    }

    private void OnSendLobbyMessage()
    {
        if (string.IsNullOrEmpty(chatInput.Text))
        {
            return;
        }
        LobbyManager.SendLobbyMessage(chatInput.Text);
        chatInput.Text = string.Empty;
    }

    public void OnLobbyMessageReceived(object sender, GlobalTypes.LobbyMessageArgs e)
    {
        chatBox.Text += e.PlayerName + ": " + e.Message + "\n";
    }
}