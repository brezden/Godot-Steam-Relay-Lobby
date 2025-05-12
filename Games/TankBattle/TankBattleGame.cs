using GodotPeer2PeerSteamCSharp.Games;

public class TankBattleGame : IGame
{
    public string GameName => "Tank Battle";
    public GlobalTypes.GameType GameType => GlobalTypes.GameType.FreeForAll;
}
