using Godot;
using System;
using GodotPeer2PeerSteamCSharp.Games;
using GodotPeer2PeerSteamCSharp.Types.Games;

namespace GodotPeer2PeerSteamCSharp.Scenes.Games.Tank;

public partial class Game : Node, GameInterface
{
    [Export] private float CloudSpeed = 10f;        // seconds to cross
    [Export] private int CloudSpawnDelay = 3;       // seconds between spawns
    [Export] private int InitialCloudCount = 5;     // how many to pre-spawn

    private int baseAndCloudWidth = 360; // Adding some padding for cloud width
    
    public string GameName => "Tank Battle";
    public GameType GameType => GameType.FreeForAll;
    
    private Sprite2D _cloudTemplate;
    private Timer _cloudSpawnTimer;

    public override void _Ready()
    {
        InitializeClouds();
    }

    private void InitializeClouds()
    {
        _cloudTemplate = GetNode<Sprite2D>("%Cloud");
        _cloudSpawnTimer = GetNode<Timer>("%CloudSpawnTimer");

        _cloudSpawnTimer.WaitTime = CloudSpawnDelay;
        _cloudSpawnTimer.Timeout += OnCloudSpawnTimeout;
        _cloudSpawnTimer.Start();

        float skyWidth = 320f;
        float spacing = skyWidth / (InitialCloudCount + 1);

        for (int i = 0; i < InitialCloudCount; i++)
        {
            float baseX = spacing * (i + 1);
            float jitterX = (float)GD.RandRange(-10, 10); // small offset
            float x = Mathf.Round(baseX + jitterX);

            float y = Mathf.Round((float)GD.RandRange(1, 13)); // random height band

            SpawnCloudAtPosition(new Vector2(x, y));
        }
    }

    private void OnCloudSpawnTimeout()
    {
        // Newly spawned clouds always start at left edge
        float y = Mathf.Round((float)GD.RandRange(1, 13));
        float x = Mathf.Round(_cloudTemplate.Position.X);
        SpawnCloudAtPosition(new Vector2(x, y));
    }

    private void SpawnCloudAtPosition(Vector2 start)
    {
        var cloud = _cloudTemplate.Duplicate() as Sprite2D;
        cloud.TextureFilter = CanvasItem.TextureFilterEnum.Nearest;
        cloud.Position = start;
        AddChild(cloud);

        var target = new Vector2(Mathf.Round(start.X + baseAndCloudWidth), start.Y);
        float duration = Mathf.Max(0.5f, CloudSpeed + (float)GD.RandRange(-1.0, 1.0));

        var tween = GetTree().CreateTween()
            .SetProcessMode(Tween.TweenProcessMode.Physics)
            .SetTrans(Tween.TransitionType.Linear)
            .SetEase(Tween.EaseType.InOut);

        tween.TweenMethod(
            Callable.From<Vector2>(v =>
                cloud.GlobalPosition = new Vector2(Mathf.Round(v.X), Mathf.Round(v.Y))),
            cloud.GlobalPosition,
            target,
            duration
        );

        tween.Finished += () => cloud.QueueFree();
    }
}
