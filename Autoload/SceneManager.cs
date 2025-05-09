﻿using Godot;
using GodotPeer2PeerSteamCSharp.Multiplayer.Types;

public partial class SceneManager : Node
{
    private static SceneManager _instance;
    public static SceneManager Instance => _instance;
    private Node _currentScene;
    
    private Node _transitionScene;
    private AnimationPlayer _transitionAnimPlayer;
    private string _pendingScenePath;

    public override void _Ready()
    {
        if (_instance != null)
        {
            QueueFree();
            return;
        }
        
        _instance = this;
        
        Viewport root = GetTree().Root;
        _currentScene = root.GetChild(root.GetChildCount() - 1);
    }

    public void GotoScene(int sceneId, SceneRegistry.SceneAnimation animationName = SceneRegistry.SceneAnimation.FadeInOut)
    {
        string path = SceneRegistry.GetScenePath(sceneId);
        _pendingScenePath = path;

        PackedScene animationScene = GD.Load<PackedScene>(SceneRegistry.SceneAnimationMapping.GetScene(animationName));
        Node animationSceneInstance = animationScene.Instantiate();
        _transitionScene = animationSceneInstance;
        
        GetTree().Root.AddChild(animationSceneInstance);
        
        AnimationPlayer animPlayer = animationSceneInstance.GetNode<AnimationPlayer>("AnimationPlayer");
        _transitionAnimPlayer = animPlayer;
        
        animPlayer.Play("start");
        animPlayer.AnimationFinished += OnAnimationFinished;
    }

    public void OpenModal(int modalId = 0)
    {
        PackedScene modalBaseScene = GD.Load<PackedScene>("res://Scenes/Components/Modal/ModalBase.tscn");
        Node modelBaseSceneInstance = modalBaseScene.Instantiate();
        GetTree().Root.AddChild(modelBaseSceneInstance);
    }
    
    private void OnAnimationFinished(StringName animationName)
    {
        if (animationName == "start")
        {
            DeferredGotoScene(_pendingScenePath);
            _transitionAnimPlayer?.Play("end");
        }
        else if (animationName == "end")
        {
            _transitionScene?.QueueFree();
            _transitionScene = null;
            _transitionAnimPlayer = null;
        }
    }

    private void DeferredGotoScene(string path)
    {
        _currentScene.Free();
        var nextScene = GD.Load<PackedScene>(path);
        _currentScene = nextScene.Instantiate();
        GetTree().Root.AddChild(_currentScene);
        GetTree().CurrentScene = _currentScene;
    }
}