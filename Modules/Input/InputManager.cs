using Godot;
using GodotPeer2PeerSteamCSharp.Games;
using Steamworks.Data;

namespace GodotPeer2PeerSteamCSharp.Autoload;

public partial class InputManager : Node
{
    private static IInputManager _currentInputManager;
    private static SendType _inputDefaultSendType;

    public static void SetInputManager(IInputManager inputManager)
    {
        _currentInputManager = inputManager;
        _inputDefaultSendType = inputManager.InputDefaultSendType;
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
