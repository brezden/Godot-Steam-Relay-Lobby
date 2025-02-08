using Godot;

public partial class SceneManager : Node
{
    private static SceneManager _instance;
    public static SceneManager Instance => _instance;

    public override void _Ready()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            QueueFree();
        }
    }

    public void ChangeScene(string scenePath)
    {
        GetTree().ChangeSceneToFile(scenePath);
    }
}