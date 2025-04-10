namespace GodotPeer2PeerSteamCSharp.Games;

public interface IInputManager
{
    void ProcessPositionalInput(int playerIndex, float x, float y);
    void ProcessActionInput(int playerIndex, string action);
}