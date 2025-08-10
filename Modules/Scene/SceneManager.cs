using Godot;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class SceneManager : Node
{
    private Node _currentScene;
    private string _pendingScenePath;
    private AnimationPlayer _transitionAnimPlayer;
    private Node _transitionScene;
    
    private CanvasLayer _backgroundLayer;
    private CanvasLayer _mainLayer;
    private CanvasLayer _uiLayer;
    private CanvasLayer _overlayLayer;
    private CanvasLayer _transitionLayer;

    public static SceneManager Instance
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

        Viewport root = GetTree().Root;
        _currentScene = root.GetChild(root.GetChildCount() - 1);
        GatherLayers();
    }
    
    private void GatherLayers()
    {
        _backgroundLayer = GetTree().Root.GetNode<CanvasLayer>("Main/BackgroundLayer");
        _mainLayer = GetTree().Root.GetNode<CanvasLayer>("Main/MainLayer");
        _uiLayer = GetTree().Root.GetNode<CanvasLayer>("Main/UILayer");
        _overlayLayer = GetTree().Root.GetNode<CanvasLayer>("Main/OverlayLayer");
        _transitionLayer = GetTree().Root.GetNode<CanvasLayer>("Main/TransitionLayer");
        
        if (_backgroundLayer == null || _mainLayer == null || _uiLayer == null ||_overlayLayer == null || _transitionLayer == null)
        {
            GD.PrintErr("One or more layers not found in the scene tree.");
        }
    }
    
    private void ClearLayers()
    {
        foreach (var child in _backgroundLayer.GetChildren())
        {
            child.QueueFree();
        }
        
        foreach (var child in _mainLayer.GetChildren())
        {
            child.QueueFree();
        }
        
        foreach (var child in _uiLayer.GetChildren())
        {
            child.QueueFree();
        }
        
        foreach (var child in _overlayLayer.GetChildren())
        {
            child.QueueFree();
        }
    }

    public async void GotoScene(int sceneId,
        SceneRegistry.SceneAnimation animationName = SceneRegistry.SceneAnimation.FadeInOut)
    {
        if (UIManager.Instance.ModalManager.IsModalShowing())
        {
            await UIManager.Instance.ModalManager.CloseModal();
        }

        var path = SceneRegistry.GetScenePath(sceneId);
        _pendingScenePath = path;

        var animationScene = GD.Load<PackedScene>(SceneRegistry.SceneAnimationMapping.GetScene(animationName));
        var animationSceneInstance = animationScene.Instantiate();
        _transitionScene = animationSceneInstance;
        var animPlayer = animationSceneInstance.GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer.Autoplay = "";

        _transitionLayer.AddChild(animationSceneInstance);
        _transitionAnimPlayer = animPlayer;

        animPlayer.Play("start");
        animPlayer.AnimationFinished += OnAnimationFinished;
    }

    private void OnAnimationFinished(StringName animationName)
    {
        if (animationName == "start")
        {
            ClearLayers();
            DeferredGotoScene(_pendingScenePath);
            _transitionAnimPlayer?.Play("end");
        }
        else if (animationName == "end")
        {
            _transitionScene?.QueueFree();
            _transitionScene = null;
            _transitionAnimPlayer = null;
        }
    }

    private void DeferredGotoScene(string path)
    {
        var nextScene = GD.Load<PackedScene>(path);
        _currentScene = nextScene.Instantiate();
        if (_currentScene == null)
        {
            GD.PrintErr($"Failed to load scene at path: {path}");
            return;
        }
        _mainLayer.AddChild(_currentScene);
    }
}
