using Godot;
using GodotPeer2PeerSteamCSharp.Autoload;
using GodotPeer2PeerSteamCSharp.Games;
using GodotPeer2PeerSteamCSharp.Games.TankBattle;
using GodotPeer2PeerSteamCSharp.Scenes.Games;
using GodotPeer2PeerSteamCSharp.Scenes.Games.Tank;

public partial class StateManager : Node
{
    private GameInterface _currentGame;

    public override void _Ready()
    {
        _currentGame = new Game();
        InputManager.SetInputManager(new TankBattleInputManager());
    }

    public void SetGame(GameInterface game)
    {
        _currentGame = game;
    }
}
