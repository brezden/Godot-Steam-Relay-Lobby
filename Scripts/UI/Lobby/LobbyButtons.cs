using Godot;
using System;
using System.Buffers.Binary;

public partial class LobbyButtons : Node
{
	public override void _Ready()
	{
		Button createLobbyButton = GetNode<Button>("HostLobby");
		Button inviteLobbyButton = GetNode<Button>("InviteMembers");
		Button sendReliableServerButton = GetNode<Button>("SendReliableServerPacket");
		Button sendUnreliableServerButton = GetNode<Button>("SendUnreliableServerPacket");
		Button sendReliableClientButton = GetNode<Button>("SendReliableClientPacket");
		Button sendUnreliableClientButton = GetNode<Button>("SendUnreliableClientPacket");
		Button sendSceneChangeButton = GetNode<Button>("SendChangeScenePacket");
		Button leaveLobbyButton = GetNode<Button>("LeaveLobby");

		createLobbyButton.Pressed += CreateLobby;
		inviteLobbyButton.Pressed += InviteLobby;
		sendReliableServerButton.Pressed += SendReliableServerPacket;
		sendUnreliableServerButton.Pressed += SendUnreliableServerPacket;
		sendReliableClientButton.Pressed += SendReliableClientPacket;
		sendUnreliableClientButton.Pressed += SendUnreliableClientPacket;
		sendSceneChangeButton.Pressed += SendSceneChangePacket; 
		leaveLobbyButton.Pressed += LeaveLobby;
	}
	
	private void CreateLobby()
	{
		LobbyManager.CreateLobby();
	}

	private void InviteLobby()
	{
		SceneManager.Instance.OpenModal(0);
	}
	
	private void SendReliableServerPacket()
	{
		TransportManager.Server.SendReliablePacket(PacketTypes.MainType.Input, 2, 1);
	}
	
	private void SendUnreliableServerPacket()
	{
		TransportManager.Server.SendUnreliablePacket(PacketTypes.MainType.Input, 2, 1, 101);
	}
	
	private void SendReliableClientPacket()
	{
		TransportManager.Client.SendReliablePacket(PacketTypes.MainType.Input, 1, 2);
	}

	private void SendUnreliableClientPacket()
	{
		TransportManager.Client.SendUnreliablePacket(PacketTypes.MainType.Input, 1, 2, 1948);
	}
	
	private void SendSceneChangePacket()
	{
		Span<byte> buffer = stackalloc byte[2];
		BinaryPrimitives.WriteUInt16LittleEndian(buffer, 100);
		TransportManager.Server.SendReliablePacket(PacketTypes.MainType.Scene, 1, 1, buffer);
		SceneManager.Instance.GotoScene(100);
	}
	
	private void LeaveLobby()
	{
		LobbyManager.LeaveLobby();
	}
}
