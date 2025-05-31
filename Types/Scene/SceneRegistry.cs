using System.Collections.Generic;

namespace GodotPeer2PeerSteamCSharp.Types.Scene;

public static class SceneRegistry
{
    public enum SceneAnimation
    {
        FadeInOut,
        SlideLeft,
        SlideRight
    }

    public static readonly Dictionary<int, string> ScenePaths = new()
    {
        { MainMenu.Home, "res://Scenes/MainMenu/main.tscn" },
        { Lobby.OnlineLobby, "res://Scenes/Lobby/Lobby.tscn" },
        { TankBattle.Game, "res://Games/TankBattle/Game.tscn" }
    };

    public static string GetScenePath(int sceneId)
    {
        var scenePath = ScenePaths.TryGetValue(sceneId, out var path);

        if (!scenePath)
            Logger.Error($"Scene ID {sceneId} not found in registry");

        return path;
    }

    public static class SceneAnimationMapping
    {
        public static readonly Dictionary<SceneAnimation, string> Map = new()
        {
            { SceneAnimation.FadeInOut, "res://Scenes/Transitions/FadeOutAndIn.tscn" },
            { SceneAnimation.SlideLeft, "res://Scenes/Transitions/SlideLeft.tscn" },
            { SceneAnimation.SlideRight, "res://Scenes/Transitions/SlideRight.tscn" }
        };

        public static string GetScene(SceneAnimation animation)
        {
            return Map[animation];
        }
    }

    public static class MainMenu
    {
        public const int Home = 0;
    }

    public static class Lobby
    {
        public const int OnlineLobby = 1;
    }

    public static class TankBattle
    {
        public const int Game = 100;
    }
}
