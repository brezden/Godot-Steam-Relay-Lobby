using Godot;
using System;
using System.Buffers.Binary;
using GodotPeer2PeerSteamCSharp.Autoload.Types.Scene;

public partial class LobbyButtons : Node
{
    public override void _Ready()
    {
        Button inviteLobbyButton = GetNode<Button>("InviteMembers");
        Button sendReliableServerButton = GetNode<Button>("SendReliableServerPacket");
        Button sendUnreliableServerButton = GetNode<Button>("SendUnreliableServerPacket");
        Button sendReliableClientButton = GetNode<Button>("SendReliableClientPacket");
        Button sendUnreliableClientButton = GetNode<Button>("SendUnreliableClientPacket");
        Button sendSceneChangeButton = GetNode<Button>("SendChangeScenePacket");
        Button leaveLobbyButton = GetNode<Button>("LeaveLobby");

        inviteLobbyButton.Pressed += InviteLobby;
        sendReliableServerButton.Pressed += SendReliableServerPacket;
        sendUnreliableServerButton.Pressed += SendUnreliableServerPacket;
        sendReliableClientButton.Pressed += SendReliableClientPacket;
        sendUnreliableClientButton.Pressed += SendUnreliableClientPacket;
        sendSceneChangeButton.Pressed += SendSceneChangePacket;
        leaveLobbyButton.Pressed += LeaveLobby;
    }

    private void InviteLobby()
    {
        SceneManager.Instance.ModalManager.ShowModal(ModalType.InvitePlayer);
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
