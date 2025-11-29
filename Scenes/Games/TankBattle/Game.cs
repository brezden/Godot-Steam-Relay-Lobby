using Godot;
using System.Collections.Generic;
using GodotPeer2PeerSteamCSharp.Types.Games;

namespace GodotPeer2PeerSteamCSharp.Scenes.Games.TankBattle;

public partial class Game : Node
{
    public string GameName => "Tank Battle";
    public GameType GameType => GameType.FreeForAll;

    [Export] private PackedScene _tankScene;
    [Export] private Node2D[] _spawnPoints;

    private readonly Dictionary<long, Tank> _peerToTank = new();

    public override void _Ready()
    {
        if (Multiplayer.IsServer())
        {
            Logger.Game("Server: Setting up game...");
            SetupGame();
        }
        else
        {
            Logger.Game("Client: Waiting for server to spawn tanks...");
        }
    }

    private void SetupGame()
    {
        int[] multiplayerIds = Multiplayer.GetPeers();
        int serverId = Multiplayer.GetUniqueId();

        var ids = new List<int> { serverId };
        ids.AddRange(multiplayerIds);

        for (int i = 0; i < ids.Count; i++)
        {
            long peerId = ids[i];
            byte playerIndex = (byte)i;

            Logger.Game($"Assigned Peer ID {peerId} to Player Index {playerIndex}");

            Vector2 spawnPosition = GetSpawnPositionForIndex(playerIndex);

            Rpc(nameof(SpawnTank), peerId, playerIndex, spawnPosition);
        }
    }

    private Vector2 GetSpawnPositionForIndex(byte playerIndex)
    {
        if (_spawnPoints != null && _spawnPoints.Length > 0)
        {
            var idx = playerIndex % _spawnPoints.Length;
            return _spawnPoints[idx].GlobalPosition;
        }

        return new Vector2(100 + 100 * playerIndex, 100);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void SpawnTank(int peerId, byte playerIndex, Vector2 position)
    {
        if (_tankScene == null)
        {
            GD.PushError("Tank scene not assigned on Game node.");
            return;
        }

        var tank = _tankScene.Instantiate<Tank>();
        tank.Position = position;

        tank.SetMultiplayerAuthority(peerId);

        tank.Name = $"Tank_{peerId}_P{playerIndex}";

        AddChild(tank);

        _peerToTank[peerId] = tank;

        GD.Print($"Spawned tank for peer {peerId} at {position} (player {playerIndex})");
    }
}
