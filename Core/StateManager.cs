using Godot;
using GodotPeer2PeerSteamCSharp.Autoload;
using GodotPeer2PeerSteamCSharp.Games;
using GodotPeer2PeerSteamCSharp.Games.TankBattle;

public partial class StateManager : Node
{
    private IGame _currentGame;

    public override void _Ready()
    {
        _currentGame = new TankBattleGame();
        InputManager.SetInputManager(new TankBattleInputManager());
    }

    public void SetGame(IGame game)
    {
        _currentGame = game;
    }
}
