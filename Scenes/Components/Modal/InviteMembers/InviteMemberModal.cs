using System.Collections.Generic;
using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

public partial class InviteMemberModal : Panel
{
    public override async void _Ready()
    {
        List<PlayerInvite> inGameFriends = LobbyManager.GetInGameFriends();
        
        Logger.Game($"In-game friends found: {inGameFriends.Count}");

        var friendListContainer = GetNode<VBoxContainer>("%FriendContainer");
        var noMemberLabel = GetNode<Label>("%NoMemberLabel");
        var closeButton = GetNode<CloseModalButton>("%CloseButton");
        var memberPanelScene = GD.Load<PackedScene>("res://Scenes/Components/Modal/InviteMembers/Member.tscn");

        closeButton.Pressed += OnCloseButtonPressed;

        foreach (var playerInvite in inGameFriends)
        {
            var memberPanel = memberPanelScene.Instantiate<MemberPanel>();
            memberPanel.Setup(
                playerInvite.PlayerId,
                playerInvite.PlayerName,
                playerInvite.PlayerStatus
            );
            friendListContainer.AddChild(memberPanel);
        }

        if (friendListContainer.GetChildCount() == 0)
        {
            friendListContainer.Hide();
            noMemberLabel.Show();
        }
        else
        {
            friendListContainer.Show();
            noMemberLabel.Hide();
        }
    }

    private void OnCloseButtonPressed()
    {
        UIManager.Instance.ModalManager.CloseModal();
    }
}
