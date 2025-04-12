using Godot;
using System;

public partial class LobbyButtons : Node
{
	public override void _Ready()
	{
		Button createLobbyButton = GetNode<Button>("HostLobby");
		Button inviteLobbyButton = GetNode<Button>("InviteMembers");
		Button sendServerPacketButton = GetNode<Button>("SendServerPacket");
		Button sendClientPacketButton = GetNode<Button>("SendClientsPacket");
		Button leaveLobbyButton = GetNode<Button>("LeaveLobby");

		createLobbyButton.Pressed += CreateLobby;
		inviteLobbyButton.Pressed += InviteLobby;
		sendServerPacketButton.Pressed += SendUnreliableServerPacket;
		sendClientPacketButton.Pressed += SendUnreliableClientPacket;
		leaveLobbyButton.Pressed += LeaveLobby;
	}
	
	private void CreateLobby()
	{
		LobbyManager.CreateLobby();
	}

	private void InviteLobby()
	{
		LobbyManager.InviteLobbyOverlay();
	}
	
	private void SendUnreliableServerPacket()
	{
		TransportManager.Server.SendUnreliablePacket(PacketTypes.MainType.Input, 2, 1, 812);
	}

	private void SendUnreliableClientPacket()
	{
		TransportManager.Client.SendUnreliablePacket(PacketTypes.MainType.Input, 1, 2, 1948);
	}
	
	private void LeaveLobby()
	{
		LobbyManager.LeaveLobby();
	}
}
