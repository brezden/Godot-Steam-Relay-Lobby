using Godot;

public partial class InputReceiver : Node
{
    private const string L = "move_left_0";
    private const string R = "move_right_0";
    private const string U = "move_up_0";
    private const string D = "move_down_0";

    public InputReceiver()
    {
        SetProcess(false);
    }
    
    public void TurnOn()
    {
        SetProcess(true);
        Logger.Game("InputReceiver Turned On");
    }
    
    public void TurnOff()
    {
        SetProcess(false);
        Logger.Game("InputReceiver Turned Off");
    }
}
