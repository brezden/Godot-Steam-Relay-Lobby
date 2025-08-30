using Godot;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class UIManager : Node
{
    private Control _uiLayer;
    private float _current = 1.0f;
    
    public static UIManager Instance
    {
        get;
        private set;
    }

    public ModalManager ModalManager
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

        ModalManager = new ModalManager();
        AddChild(ModalManager);
        _uiLayer = GetTree().Root.GetNode<Control>("Main/UILayer/UIControlLayer");
        ApplyUiScale(_current);
    }
    
    public void ApplyUiScale(float factor)
    {
        _current = factor;

        var vpSize = GetViewport().GetVisibleRect().Size;
        _uiLayer.Scale = Vector2.One * factor;

        // Counter-resize so, after scaling, it still covers the viewport.
        var logical = vpSize / factor;
        _uiLayer.CustomMinimumSize = logical;
        _uiLayer.Size = logical;      // keep anchors but ensure layout rect matches
        _uiLayer.PivotOffset = Vector2.Zero; // scale from top-left; or use Size/2 for center
    }
}
