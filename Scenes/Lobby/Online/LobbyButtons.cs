using System;
using System.Buffers.Binary;
using Godot;
using GodotPeer2PeerSteamCSharp.Modules;
using GodotPeer2PeerSteamCSharp.Modules.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class LobbyButtons : Node
{
    public override void _Ready()
    {
        var inviteLobbyButton = GetNode<Button>("InviteMembers");
        var leaveLobbyButton = GetNode<Button>("LeaveLobby");

        inviteLobbyButton.Pressed += InviteLobby;
        leaveLobbyButton.Pressed += LeaveLobby;
    }

    private void InviteLobby()
    {
        UIManager.Instance.ModalManager.RenderModal(ModalType.InvitePlayer);
    }

    private void LeaveLobby()
    {
        LobbyManager.LeaveLobbyAndTransport();
        SceneManager.Instance.GotoScene(SceneRegistry.MainMenu.Home);
    }
}
