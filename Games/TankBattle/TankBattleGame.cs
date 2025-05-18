using GodotPeer2PeerSteamCSharp.Games;
using GodotPeer2PeerSteamCSharp.Types.Games;

public class TankBattleGame : IGame
{
    public string GameName => "Tank Battle";
    public GameType GameType => GameType.FreeForAll;
}
