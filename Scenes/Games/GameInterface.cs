using GodotPeer2PeerSteamCSharp.Types.Games;

namespace GodotPeer2PeerSteamCSharp.Games;

public interface GameInterface
{
    string GameName
    {
        get;
    }

    GameType GameType
    {
        get;
    }
}
