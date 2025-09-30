using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Input;
using GodotPeer2PeerSteamCSharp.Scenes.Games.TankBattle;
using Steamworks.Data;

namespace GodotPeer2PeerSteamCSharp.Games.TankBattle;

public partial class TankGameInputManager : Node, IInputHandler
{
    public Game currentGame { get; }
    public SendType InputDefaultSendType => SendType.Unreliable;

    public TankGameInputManager (Game game)
    {
        currentGame = game;
    }
    
    // TODO FIGURE OUT HOW TO STORE THE PLAYER INDEX
    public void ProcessPositionalInput(byte playerIndex, Vector2 input, double delta)
    {
        Logger.Game($"Processing input for player {playerIndex}: {input} with delta {delta}");
        currentGame.PlayerProcessedInput(playerIndex, input, delta);
    }
}
