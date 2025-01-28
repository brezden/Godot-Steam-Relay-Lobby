using Godot;
using System;

public partial class Buttons : Node
{
	public override void _Ready()
	{
		Button createLobbyButton = GetNode<Button>("CreateLobbyButton");

		createLobbyButton.Pressed += CreateLobby;

	}

	public override void _Process(double delta)
	{
	}
	
	private void CreateLobby()
	{
		MultiplayerManager.CreateLobby();
	}
}
