namespace GodotPeer2PeerSteamCSharp.Games;
using Steamworks.Data;

public interface IInputManager
{
    SendType InputDefaultSendType { get; }
    void ProcessPositionalInput(int playerIndex, float x, float y);
    void ProcessActionInput(int playerIndex, string action);
}