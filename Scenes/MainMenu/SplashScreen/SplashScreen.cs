using Godot;
using System;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class SplashScreen : CanvasLayer
{
    private AnimationPlayer animationPlayer;
    
	public override void _Ready()
	{
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.AnimationFinished += (_) => OnSplashScreenCompleted();
    }
    
    private void OnSplashScreenCompleted()
    {
        SceneManager.Instance.GotoScene(SceneRegistry.MainMenu.Home);
    }
}
