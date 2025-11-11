using Godot;
using System.Collections.Generic;
using GodotPeer2PeerSteamCSharp.Autoload;
using GodotPeer2PeerSteamCSharp.Games;
using GodotPeer2PeerSteamCSharp.Games.TankBattle;
using GodotPeer2PeerSteamCSharp.Types.Games;

namespace GodotPeer2PeerSteamCSharp.Scenes.Games.TankBattle;

public partial class Game : Node, IGame
{
    public string GameName => "Tank Battle";
    public GameType GameType => GameType.FreeForAll;

    private Tank _playerOneTank;
    private Tank _playerTwoTank;

    public override void _Ready()
    {
        InputManager.Instance.SetInputHandler(new TankGameInputManager(this));
        _playerOneTank = GetNode<Tank>("%PlayerOne");
        _peerToPlayerIndex[1] = 0;
        StartGame();
    }

    private readonly Dictionary<long, byte> _peerToPlayerIndex = new();

    public override void _Process(double delta)
    {
        Vector2 inputVector = Input.GetVector("move_left_0","move_right_0","move_up_0","move_down_0");

        long myPeerId = Multiplayer.GetUniqueId();
        if (!_peerToPlayerIndex.TryGetValue(myPeerId, out var myPlayerIndex))
            return;

        Rpc(nameof(AllApplyInput), myPlayerIndex, inputVector);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer,
        TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable,
        CallLocal = true)]
    private void AllApplyInput(byte playerIndex, Vector2 input)
    {
        PlayerProcessedInput(playerIndex, input, GetProcessDeltaTime());
    }

    public override void _ExitTree()
    {
        InputManager.Instance.StopReceivingInput();
    }

    public void StartGame()
    {
        Logger.Game("Starting Tank Battle Game...");
        InputManager.Instance.StartReceivingInput();
    }

    public void PlayerProcessedInput(byte playerIndex, Vector2 input, double delta)
    {
        switch (playerIndex)
        {
            case 0:
                _playerOneTank?.ProcessInput(input, delta);
                break;
            case 1:
                _playerTwoTank?.ProcessInput(input, delta);
                break;
            default:
                Logger.Error($"Unhandled player index: {playerIndex}");
                break;
        }
    }
}
