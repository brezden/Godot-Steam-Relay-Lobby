using Godot;
using System;
using GodotPeer2PeerSteamCSharp.Autoload.Types.Scene;
using GodotPeer2PeerSteamCSharp.Multiplayer.Types;

public partial class MainMenuButtons : Node
{
	public override void _Ready()
	{
		Button hostOnlineButton = GetNode<Button>("HostOnline");
		Button localPlayButton = GetNode<Button>("LocalPlay");
		Button exitButton = GetNode<Button>("ExitGame");
		
		hostOnlineButton.Pressed += HostOnline;
		localPlayButton.Pressed += LocalPlay;
		exitButton.Pressed += ExitGame;
	}
	
	private void HostOnline()
	{
		SceneManager.Instance.ModalManager.showInformationModal("Creating a lobby", InformationModalType.Loading);
		EventBus.Lobby.OnCreateLobby();
	}
	
	private void LocalPlay()
	{
	}
	
	private void ExitGame()
	{
		GetTree().Quit();
	}
}
