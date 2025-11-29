using Godot;

public partial class GameContainer : Control
{
    [Export] public SubViewport SubVp;
    [Export] public TextureRect Output;
    [Export] public Vector2I BaseSize = new(320, 180);

    public override void _Ready()
    {
        SubVp.Size = BaseSize;
        SubVp.RenderTargetUpdateMode = SubViewport.UpdateMode.Always;
        SubVp.RenderTargetClearMode = SubViewport.ClearMode.Always;

        SubVp.CanvasItemDefaultTextureFilter = Viewport.DefaultCanvasItemTextureFilter.Nearest;
        SubVp.CanvasItemDefaultTextureRepeat = Viewport.DefaultCanvasItemTextureRepeat.Disabled;
        SubVp.Snap2DTransformsToPixel = true;
        SubVp.Snap2DVerticesToPixel = true;

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
        Vector2 win = GetViewportRect().Size;

        float sx = win.X / BaseSize.X;
        float sy = win.Y / BaseSize.Y;
        float k = Mathf.Min(sx, sy); // float now

        Vector2 outSize = (Vector2)BaseSize * k;
        Vector2 outPos = (win - outSize) / 2.0f;

        Output.Size = outSize;
        Output.Position = outPos;
    }

}