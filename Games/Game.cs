namespace GodotPeer2PeerSteamCSharp.Games;

public interface IGame
{
    string GameName
    {
        get;
    }

    GlobalTypes.GameType GameType
    {
        get;
    }
}
