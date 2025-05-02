using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class InviteMemberModal : Panel
{
	public override void _Ready()
	{
		Task<List<GlobalTypes.PlayerInvite>> inGameFriends = LobbyManager.GetInGameFriends();
		foreach (GlobalTypes.PlayerInvite playerInvite in inGameFriends.Result)
		{
			Logger.Game($"Player invite: {playerInvite.PlayerName}");
		}
	}
}
