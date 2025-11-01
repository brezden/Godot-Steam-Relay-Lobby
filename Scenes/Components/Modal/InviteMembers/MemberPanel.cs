using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Lobby;

public partial class MemberPanel : Panel
{
    public ulong PlayerId
    {
        get;
        set;
    }

    public string PlayerName
    {
        get;
        set;
    }

    public ImageTexture PlayerPicture
    {
        get;
        set;
    }

    public string PlayerStatus
    {
        get;
        set;
    }

    public override void _Ready()
    {
        var inviteButton = GetNode<Button>("%InviteButton");
        inviteButton.Pressed += OnInviteButtonPressed;
    }

    public void Setup(ulong playerId, string playerName, string playerStatus, ImageTexture playerPicture = null)
    {
        PlayerId = playerId;
        PlayerName = playerName;
        PlayerStatus = playerStatus;
        PlayerPicture = playerPicture;

        var playerNameLabel = GetNode<Label>("%PlayerName");
        var playerStatusLabel = GetNode<Label>("%PlayerStatus");

        if (PlayerPicture != null)
        {
            var playerPictureRect = GetNode<TextureRect>("%PlayerPicture");
            playerPictureRect.Texture = PlayerPicture;
        }

        playerNameLabel.Text = PlayerName;
        playerStatusLabel.Text = PlayerStatus;
    }

    public void OnInviteButtonPressed()
    {
        LobbyManager.InvitePlayer(PlayerId);
    }
}
