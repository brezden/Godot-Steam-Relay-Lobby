using Godot;

public partial class GameContainer : Control
{
    [Export] public SubViewport SubVp;
    [Export] public TextureRect Output;
    [Export] public Vector2I BaseSize = new(320, 180);

    public override void _Ready()
    {
        // SubViewport
        SubVp.Size = BaseSize;
        SubVp.RenderTargetUpdateMode = SubViewport.UpdateMode.Always;
        SubVp.RenderTargetClearMode = SubViewport.ClearMode.Always;
        SubVp.CanvasItemDefaultTextureFilter = Viewport.DefaultCanvasItemTextureFilter.Nearest;
        SubVp.CanvasItemDefaultTextureRepeat = Viewport.DefaultCanvasItemTextureRepeat.Disabled;
        SubVp.Snap2DTransformsToPixel = true;
        SubVp.Snap2DVerticesToPixel = true;

        // TextureRect
        Output.Texture = SubVp.GetTexture();
        Output.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
        Output.TextureFilter = CanvasItem.TextureFilterEnum.Nearest;

        ResizeOutput();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationResized) ResizeOutput();
    }

    private void ResizeOutput()
    {
        // integer window size
        Vector2 winF = GetViewportRect().Size;
        Vector2I win = new(Mathf.FloorToInt(winF.X), Mathf.FloorToInt(winF.Y));

        // biggest integer scale that fits
        int sx = win.X / BaseSize.X;
        int sy = win.Y / BaseSize.Y;
        int k = Mathf.Max(1, Mathf.Min(sx, sy));

        Vector2I outSize = new(BaseSize.X * k, BaseSize.Y * k);
        Vector2I outPos = (win - outSize) / 2; // integer center

        Output.CustomMinimumSize = (Vector2)outSize;
        Output.Size = (Vector2)outSize;
        Output.Position = (Vector2)outPos; // whole pixels
    }
}