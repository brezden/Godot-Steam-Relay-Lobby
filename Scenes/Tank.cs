using Godot;
using System;
using System.Collections.Generic;

public partial class Tank : Node2D
{
    [Export] public int PlayerIndex { get; set; } = 1; // 1 to 4

    private static readonly Dictionary<int, (Vector2I body, Vector2I top)> FrameMap =
        new Dictionary<int, (Vector2I, Vector2I)>
        {
            { 1, (new Vector2I(0, 0), new Vector2I(1, 0)) },
            { 2, (new Vector2I(0, 1), new Vector2I(1, 1)) },
            { 3, (new Vector2I(0, 2), new Vector2I(1, 2)) },
            { 4, (new Vector2I(0, 3), new Vector2I(1, 3)) },
        };

    public override void _Ready()
    {
        SetPlayerFrame(PlayerIndex);
    }

    private void SetPlayerFrame(int index)
    {
        var body = GetNode<Sprite2D>("Body");
        var top = GetNode<Sprite2D>("Top");

        if (FrameMap.TryGetValue(index, out var frames))
        {
            body.FrameCoords = frames.body;
            top.FrameCoords = frames.top;
        }
        else
        {
            GD.PushWarning($"Invalid PlayerIndex: {index}. Defaulting to Player 1.");
            body.FrameCoords = FrameMap[1].body;
            top.FrameCoords = FrameMap[1].top;
        }
    }
}
