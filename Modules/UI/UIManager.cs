using Godot;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class UIManager : Node
{
    private CanvasLayer _uiLayer;
    
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
        _uiLayer = GetTree().Root.GetNode<CanvasLayer>("Main/UILayer");
    }
}
