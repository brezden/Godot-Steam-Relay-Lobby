namespace GodotPeer2PeerSteamCSharp.Autoload;
using GodotPeer2PeerSteamCSharp.Games;
using Godot;

public partial class InputManager : Node
{
    private static IInputManager _currentInputManager;
    
    public static void SetInputManager(IInputManager inputManager)
    {
        _currentInputManager = inputManager;
    }
   
    public void ProcessPositionalInput(int playerIndex, float x, float y)
    {
        _currentInputManager.ProcessPositionalInput(playerIndex, x, y);
    }
    
    public void ProcessActionInput(int playerIndex, string action)
    {
        _currentInputManager.ProcessActionInput(playerIndex, action);
    }
}