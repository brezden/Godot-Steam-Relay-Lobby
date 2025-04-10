using Godot;
using System;

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
	
	PackedScene lobbyScene = (PackedScene)ResourceLoader.Load("res://Scenes/Lobby/Lobby.tscn");
	
	private void HostOnline()
	{
		SceneManager.Instance.GotoScene("res://Scenes/Lobby/Lobby.tscn");
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
