using Godot;
using GodotPeer2PeerSteamCSharp.Games;
using Steamworks.Data;

namespace GodotPeer2PeerSteamCSharp.Modules.Input;

public interface IInputHandler
{
    public SendType InputDefaultSendType { get; }
    public void ProcessPositionalInput(byte playerIndex, Vector2 input, double delta);
}