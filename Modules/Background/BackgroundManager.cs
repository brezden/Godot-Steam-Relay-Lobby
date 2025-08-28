using Godot;

namespace GodotPeer2PeerSteamCSharp.Modules.Background;

public partial class BackgroundManager: Node
{
    private const string ShapeTransformPath = "res://Scenes/Backgrounds/ShapeTransform.tscn";
    private CanvasLayer _backgroundLayer;

    public static BackgroundManager Instance
    {
        get;
        private set;
    }
    
    public override void _Ready()
    {
        if (Instance != null)
        {
            QueueFree();
            return;
        }

        Instance = this;
        _backgroundLayer = GetTree().Root.GetNode<CanvasLayer>("Main/BackgroundLayer");
    }
    
    /**
     * Loads a solid color background.
     *
     * @param Color The color to fill the background with.
     */
    public void LoadColourBackground(Color color)
    {
        ClearBackground();
        var colorRect = new ColorRect
        {
            Color = color,
        };
        colorRect.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        _backgroundLayer.AddChild(colorRect);
    }
    
    /**
     * Loads the ShapeTransform background.
     */
    public void LoadShapeTransform()
    {
        ClearBackground();
        var shapeTransformScene = GD.Load<PackedScene>(ShapeTransformPath);
        if (shapeTransformScene == null)
        {
            GD.PrintErr("Failed to load ShapeTransform scene.");
            return;
        }

        var shapeTransformInstance = shapeTransformScene.Instantiate();
        if (shapeTransformInstance == null)
        {
            GD.PrintErr("Failed to instantiate ShapeTransform scene.");
            return;
        }
        
        _backgroundLayer.AddChild(shapeTransformInstance);
    }
    
    private void ClearBackground()
    {
        foreach (var child in _backgroundLayer.GetChildren())
        {
            child.QueueFree();
        }
    }
}
