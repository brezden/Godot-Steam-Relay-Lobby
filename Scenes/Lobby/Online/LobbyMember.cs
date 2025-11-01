using Godot;

public partial class LobbyMember : Control
{
    private RichTextLabel _nameLabel;
    private TextureRect _profilePicture;
    
    [Export]
    public string PlayerName
    {
        get;
        set;
    } = "Unknown";

    [Export]
    public Texture2D ProfilePicture
    {
        get;
        set;
    }

    public override void _Ready()
    {
        _nameLabel = GetNodeOrNull<RichTextLabel>("%NameLabel");
        _profilePicture = GetNodeOrNull<TextureRect>("%ProfilePicture");

        if (_nameLabel == null)
        {
            Logger.Error("NameLabel node not found in LobbyMember scene!");
            return;
        }

        if (_profilePicture == null)
        {
            Logger.Error("ProfilePicture node not found in LobbyMember scene!");
            return;
        }

        _nameLabel.Text = $"[center]{PlayerName}[/center]";
        _profilePicture.Texture = ProfilePicture;
    }
}
