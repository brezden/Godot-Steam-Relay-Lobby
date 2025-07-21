using Godot;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class SceneManager : Node
{
    public CanvasLayer UICanvasLayer;
    
    private Node _currentScene;
    private string _pendingScenePath;
    private AnimationPlayer _transitionAnimPlayer;
    private Node _transitionScene;

    public static SceneManager Instance
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
        
        UICanvasLayer = GetNode<CanvasLayer>("UICanvasLayer");

        ModalManager = new ModalManager();
        AddChild(ModalManager);
        Viewport root = GetTree().Root;
        _currentScene = root.GetChild(root.GetChildCount() - 1);
    }

    public async void GotoScene(int sceneId,
        SceneRegistry.SceneAnimation animationName = SceneRegistry.SceneAnimation.FadeInOut)
    {
        await ModalManager.CloseModal();

        var path = SceneRegistry.GetScenePath(sceneId);
        _pendingScenePath = path;

        var animationScene = GD.Load<PackedScene>(SceneRegistry.SceneAnimationMapping.GetScene(animationName));
        var animationSceneInstance = animationScene.Instantiate();
        _transitionScene = animationSceneInstance;

        GetTree().Root.AddChild(animationSceneInstance);

        var animPlayer = animationSceneInstance.GetNode<AnimationPlayer>("AnimationPlayer");
        _transitionAnimPlayer = animPlayer;

        animPlayer.Play("start");
        animPlayer.AnimationFinished += OnAnimationFinished;
    }

    private void OnAnimationFinished(StringName animationName)
    {
        if (animationName == "start")
        {
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
        _currentScene.Free();
        var nextScene = GD.Load<PackedScene>(path);
        _currentScene = nextScene.Instantiate();
        GetTree().Root.AddChild(_currentScene);
        GetTree().CurrentScene = _currentScene;
    }
}
