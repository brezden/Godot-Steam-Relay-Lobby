using Godot;
using System;
using GodotPeer2PeerSteamCSharp.Multiplayer.Types;

public partial class MainMenuButtons : Node
{
	public override void _Ready()
	{
		Button hostOnlineButton = GetNode<Button>("HostOnline");
		Button localPlayButton = GetNode<Button>("LocalPlay");
		Button settingsButton = GetNode<Button>("Settings");
		Button exitButton = GetNode<Button>("ExitGame");
		
		hostOnlineButton.Pressed += HostOnline;
		localPlayButton.Pressed += LocalPlay;
		settingsButton.Pressed += Settings;
		exitButton.Pressed += ExitGame;
	}
	
	private void HostOnline()
	{
		SceneManager.Instance.GotoScene(SceneRegistry.Lobby.OnlineLobby);
	}
	
	private void LocalPlay()
	{
	}
	
	private void Settings()
	{
	}
	
	private void ExitGame()
	{
		GetTree().Quit();
	}
}
