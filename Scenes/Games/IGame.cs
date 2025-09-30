using GodotPeer2PeerSteamCSharp.Types.Games;

namespace GodotPeer2PeerSteamCSharp.Games;

public interface IGame
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
