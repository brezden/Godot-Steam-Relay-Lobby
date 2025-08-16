using Godot;

public partial class GameContainer : Control
{
    [Export] public SubViewport SubVp;
    [Export] public TextureRect Output;
    [Export] public Vector2I BaseSize = new(320, 180);

    public override void _Ready()
    {
        SubVp.Size = BaseSize;
        Output.Texture = SubVp.GetTexture();
        Output.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
        Output.TextureFilter = CanvasItem.TextureFilterEnum.Nearest;
        ResizeOutput();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationResized)
            ResizeOutput();
    }

    private void ResizeOutput()
    {
        var win = GetViewportRect().Size;

        // biggest integer scale that fits in both axes
        int fitX = Mathf.FloorToInt(win.X / (float)BaseSize.X);
        int fitY = Mathf.FloorToInt(win.Y / (float)BaseSize.Y);
        int scale = Mathf.Max(1, Mathf.Min(fitX, fitY));

        Vector2 size = new Vector2(BaseSize.X * scale, BaseSize.Y * scale);

        // Center the scaled output
        Output.CustomMinimumSize = size;
        Output.Size = size;
        Output.Position = (win - size) * 0.5f;
    }
}