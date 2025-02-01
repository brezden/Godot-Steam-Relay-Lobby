using Godot;
using System;

public partial class LobbyMemberContainer : Node
{
	public override void _Ready()
	{
		MultiplayerManager.PlayerJoinedLobby += OnPlayerJoinedLobby;
	}

	private void OnPlayerJoinedLobby(object sender, MultiplayerManager.PlayerInformationArgs args)
	{
		GD.Print("Player joined lobby: " + args.PlayerName);
	}
}
