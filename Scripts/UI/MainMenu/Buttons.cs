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
		LobbyManager.CreateLobby();
	}

	private void InviteLobby()
	{
		LobbyManager.InviteLobbyOverlay();
	}
}
