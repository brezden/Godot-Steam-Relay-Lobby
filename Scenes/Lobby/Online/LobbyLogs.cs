using Godot;
using System;

public partial class LobbyLogs : TextEdit
{
    [Export] public bool AutoScrollToBottom = true;

    public override void _Ready()
    {
        Editable = false;
        EventBus.Lobby.LobbyLog += OnLobbyLog;
    }

    public override void _ExitTree()
    {
        EventBus.Lobby.LobbyLog -= OnLobbyLog;
    }

    private void OnLobbyLog(object? sender, string message)
    {
        var ts = DateTime.Now.ToString("HH:mm:ss");
        AppendLine($"[{ts}] {message}");
    }

    private void AppendLine(string line)
    {
        Text += line + "\n";

        if (AutoScrollToBottom)
            CallDeferred(nameof(ScrollToBottom));
    }

    private void ScrollToBottom()
    {
        int last = Math.Max(GetLineCount() - 1, 0);
        SetCaretLine(last, false, false);
        SetCaretColumn(GetLine(last).Length);
    }
}