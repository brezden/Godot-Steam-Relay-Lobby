using GodotPeer2PeerSteamCSharp.Modules.Input;
using Steamworks.Data;

namespace GodotPeer2PeerSteamCSharp.Games.TankBattle;

public class TankGameInputManager : IInputHandler
{
    public SendType InputDefaultSendType => SendType.Unreliable;

    public void ProcessPositionalInput(byte playerIndex, short x, short y)
    {
        Logger.Game($"Player {playerIndex} moved to position ({x}, {y})");
    }
}
