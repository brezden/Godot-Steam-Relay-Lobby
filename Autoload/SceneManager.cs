using Godot;
using GodotPeer2PeerSteamCSharp.Multiplayer.Types;

public partial class SceneManager : Node
{
    private static SceneManager _instance;
    public static SceneManager Instance => _instance;
    private Node _currentScene;

    public override void _Ready()
    {
        if (_instance != null)
        {
            QueueFree();
            return;
        }
        
        _instance = this;
        
        Viewport root = GetTree().Root;
        _currentScene = root.GetChild(root.GetChildCount() - 1);
    }

    public void GotoScene(int sceneId)
    {
        string path = SceneRegistry.GetScenePath(sceneId);
        Logger.Game($"Loading scene: {path}");
        CallDeferred(MethodName.DeferredGotoScene, path);
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