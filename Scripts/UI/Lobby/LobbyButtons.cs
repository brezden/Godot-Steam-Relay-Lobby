using System;
using System.Buffers.Binary;
using Godot;
using GodotPeer2PeerSteamCSharp.Core;
using GodotPeer2PeerSteamCSharp.Core.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class LobbyButtons : Node
{
    public override void _Ready()
    {
        var inviteLobbyButton = GetNode<Button>("InviteMembers");
        var sendReliableServerButton = GetNode<Button>("SendReliableServerPacket");
        var sendUnreliableServerButton = GetNode<Button>("SendUnreliableServerPacket");
        var sendReliableClientButton = GetNode<Button>("SendReliableClientPacket");
        var sendUnreliableClientButton = GetNode<Button>("SendUnreliableClientPacket");
        var sendSceneChangeButton = GetNode<Button>("SendChangeScenePacket");
        var leaveLobbyButton = GetNode<Button>("LeaveLobby");

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
        SceneManager.Instance.GotoScene(SceneRegistry.MainMenu.Home);
    }
}
