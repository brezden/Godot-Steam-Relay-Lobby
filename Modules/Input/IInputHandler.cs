using Steamworks.Data;

namespace GodotPeer2PeerSteamCSharp.Modules.Input;

public interface IInputHandler
{
    public SendType InputDefaultSendType { get; }
    public void ProcessPositionalInput(byte playerIndex, short x, short y);
}