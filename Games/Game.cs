namespace GodotPeer2PeerSteamCSharp.Games;

public interface IGame
{
    string GameName { get; }
    int MaxPlayerCount { get; }
}