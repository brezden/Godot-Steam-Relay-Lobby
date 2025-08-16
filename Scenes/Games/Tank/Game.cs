using Godot;
using GodotPeer2PeerSteamCSharp.Games;
using GodotPeer2PeerSteamCSharp.Types.Games;

namespace GodotPeer2PeerSteamCSharp.Scenes.Games.Tank;

public partial class Game : Node, GameInterface
{
    public string GameName => "Tank Battle";
    public GameType GameType => GameType.FreeForAll;

    private Sprite2D Cloud;
    
    public override void _Ready()
    {
        Cloud = GetNode<Sprite2D>("%Cloud");
    }
}
