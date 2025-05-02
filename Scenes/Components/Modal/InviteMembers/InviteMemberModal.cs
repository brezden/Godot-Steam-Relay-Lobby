using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class InviteMemberModal : Panel
{
	public override async void _Ready()
	{
		var inGameFriends = await LobbyManager.GetInGameFriends();

		var friendListContainer = GetNode<VBoxContainer>("%FriendContainer");
		var memberPanelScene = GD.Load<PackedScene>("res://Scenes/Components/Modal/InviteMembers/Member.tscn");

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
	}
}
