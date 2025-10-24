using Godot;
using GodotPeer2PeerSteamCSharp.Games;

namespace GodotPeer2PeerSteamCSharp.Modules.Input;

public interface IInputHandler
{
    public void ProcessPositionalInput(byte playerIndex, Vector2 input, double delta);
}