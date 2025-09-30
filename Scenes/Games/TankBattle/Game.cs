using Godot;
using System;
using GodotPeer2PeerSteamCSharp.Autoload;
using GodotPeer2PeerSteamCSharp.Games;
using GodotPeer2PeerSteamCSharp.Games.TankBattle;
using GodotPeer2PeerSteamCSharp.Types.Games;

namespace GodotPeer2PeerSteamCSharp.Scenes.Games.Tank;

public partial class Game : Node, GameInterface
{
    public string GameName => "Tank Battle";
    public GameType GameType => GameType.FreeForAll;

    public override void _Ready()
    {
        InputManager.CurrentInputHandler = new TankGameInputManager();
    }
}
