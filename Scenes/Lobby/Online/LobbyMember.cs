using Godot;

public partial class LobbyMember : Control
{
    private RichTextLabel _nameLabel;
    private TextureRect _profilePicture;

    private string _playerName = "Unknown";
    private Texture2D _profileTexture;

    [Export]
    public string PlayerName
    {
        get => _playerName;
        set
        {
            _playerName = value ?? "Unknown";
            UpdateNameLabel();
        }
    }

    [Export]
    public Texture2D ProfilePicture
    {
        get => _profileTexture;
        set
        {
            _profileTexture = value;
            UpdateProfilePicture();
        }
    }

    public override void _Ready()
    {
        _nameLabel = GetNodeOrNull<RichTextLabel>("%NameLabel");
        _profilePicture = GetNodeOrNull<TextureRect>("%ProfilePicture");

        if (_nameLabel == null)
            Logger.Error("NameLabel node not found in LobbyMember scene!");
        if (_profilePicture == null)
            Logger.Error("ProfilePicture node not found in LobbyMember scene!");

        UpdateNameLabel();
        UpdateProfilePicture();
    }

    private void UpdateNameLabel()
    {
        if (!IsInsideTree() || _nameLabel == null) return;

        _nameLabel.Text = $"[center]{_playerName}[/center]";
    }

    private void UpdateProfilePicture()
    {
        if (!IsInsideTree() || _profilePicture == null) return;

        _profilePicture.Texture = _profileTexture;
    }
}