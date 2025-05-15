using Steamworks.Data;

namespace GodotPeer2PeerSteamCSharp.Games;

public interface IInputManager
{
    SendType InputDefaultSendType
    {
        get;
    }

    void ProcessPositionalInput(int playerIndex, float x, float y);
    void ProcessActionInput(int playerIndex, string action);
}
