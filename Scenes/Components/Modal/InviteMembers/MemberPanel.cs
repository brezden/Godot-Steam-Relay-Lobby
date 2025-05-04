using Godot;
using System;

public partial class MemberPanel : Panel
{
	public string PlayerId { get; set; }
	public string PlayerName { get; set; }
	public ImageTexture PlayerPicture { get; set; }
	public string PlayerStatus { get; set; }
	
	public override void _Ready()
	{
		Button inviteButton = GetNode<Button>("%InviteButton");
		inviteButton.Pressed += OnInviteButtonPressed;
	}

	public void Setup(string playerId, string playerName, ImageTexture playerPicture, string playerStatus)
	{
		PlayerId = playerId;
		PlayerName = playerName;
		PlayerPicture = playerPicture;
		PlayerStatus = playerStatus;
		
		Label playerNameLabel = GetNode<Label>("%PlayerName");
		Label playerStatusLabel = GetNode<Label>("%PlayerStatus");
		TextureRect playerPictureRect = GetNode<TextureRect>("%PlayerPicture");
		
		playerNameLabel.Text = PlayerName;
		playerStatusLabel.Text = PlayerStatus;
		playerPictureRect.Texture = PlayerPicture;
	}

	public void OnInviteButtonPressed()
	{
		LobbyManager.InvitePlayer(PlayerId);
	}
}
