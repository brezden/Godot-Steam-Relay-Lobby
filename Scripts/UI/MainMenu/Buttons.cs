using Godot;
using System;

public partial class Buttons : Node
{
	public override void _Ready()
	{
		Button createLobbyButton = GetNode<Button>("HostLobby");
		Button inviteLobbyButton = GetNode<Button>("InviteMembers");
		Button sendServerPacketButton = GetNode<Button>("SendServerPacket");
		Button sendClientPacketButton = GetNode<Button>("SendClientsPacket");
		Button sendLobbyMessageButton = GetNode<Button>("SendLobbyMessage");

		createLobbyButton.Pressed += CreateLobby;
		inviteLobbyButton.Pressed += InviteLobby;
		sendServerPacketButton.Pressed += SendServerPacket;
		sendClientPacketButton.Pressed += SendClientPacket;
		sendLobbyMessageButton.Pressed += SendLobbyMessage;
	}
	
	private void CreateLobby()
	{
		LobbyManager.CreateLobby();
	}

	private void InviteLobby()
	{
		LobbyManager.InviteLobbyOverlay();
	}
	
	private void SendServerPacket()
	{
		TransportManager.SendPacketToServer(PacketTypes.MainType.Player, 69, new byte[] { 0, 1, 2, 3 });
	}

	private void SendClientPacket()
	{
		TransportManager.SendPacketToClients(PacketTypes.MainType.Player, 69, new byte[] { 0, 1, 2, 3 });
	}
	
	private void SendLobbyMessage()
	{
		LobbyManager.SendLobbyMessage("Yoooooooooooooooooooooo");
	}
}
