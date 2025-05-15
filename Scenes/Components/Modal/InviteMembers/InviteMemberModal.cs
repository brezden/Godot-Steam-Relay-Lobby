using Godot;

public partial class InviteMemberModal : Panel
{
    public override async void _Ready()
    {
        var inGameFriends = await LobbyManager.GetInGameFriends();

        var friendListContainer = GetNode<VBoxContainer>("%FriendContainer");
        var noMemberLabel = GetNode<Label>("%NoMemberLabel");
        var closeButton = GetNode<Button>("%CloseButton");
        var memberPanelScene = GD.Load<PackedScene>("res://Scenes/Components/Modal/InviteMembers/Member.tscn");

        closeButton.Pressed += OnCloseButtonPressed;

        foreach (var playerInvite in inGameFriends)
        {
            var memberPanel = memberPanelScene.Instantiate<MemberPanel>();
            memberPanel.Setup(
                playerInvite.PlayerId,
                playerInvite.PlayerName,
                playerInvite.PlayerPicture,
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
        SceneManager.Instance.ModalManager.CloseModal();
    }
}
