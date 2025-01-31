using Godot;
using System;

public partial class Buttons : Node
{
	public override void _Ready()
	{
		Button createLobbyButton = GetNode<Button>("HostLobby");
		Button inviteLobbyButton = GetNode<Button>("InviteMembers");

		createLobbyButton.Pressed += CreateLobby;
		inviteLobbyButton.Pressed += InviteLobby;

	}

	public override void _Process(double delta)
	{
	}
	
	private void CreateLobby()
	{
		MultiplayerManager.CreateLobby();
	}

	private void InviteLobby()
	{
		MultiplayerManager.InviteLobbyOverlay();
	}
}
