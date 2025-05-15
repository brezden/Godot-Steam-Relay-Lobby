using Godot;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class InformationModal : Panel
{
    private Button CloseButton;
    private Label DescriptionLabel;
    public string descriptionText;
    private Control ErrorIcon;

    private Label HeaderLabel;
    public string headerText;
    private Control LoadingSpinner;
    public InformationModalType type;

    public override void _Ready()
    {
        HeaderLabel = GetNode<Label>("%Header");
        DescriptionLabel = GetNode<Label>("%Description");
        LoadingSpinner = GetNode<Control>("%LoadingIcon");
        ErrorIcon = GetNode<Control>("%ErrorIcon");
        CloseButton = GetNode<Button>("%CloseButton");

        setHeaderText(headerText);
        setDescriptionText(descriptionText);
        setType(type);
    }

    public void prepareModal(string header, InformationModalType type, string description = null)
    {
        headerText = header;
        this.type = type;
        descriptionText = description;
    }

    public void UpdateModal(string header, InformationModalType type, string description = null)
    {
        headerText = header;
        this.type = type;
        descriptionText = description;

        setHeaderText(headerText);
        setType(type);
        setDescriptionText(descriptionText);
    }

    private void setHeaderText(string text)
    {
        if (text != null)
        {
            HeaderLabel.Text = text;
            HeaderLabel.Visible = true;
        }
        else
        {
            HeaderLabel.Visible = false;
        }
    }

    private void setDescriptionText(string text)
    {
        if (text != null)
        {
            DescriptionLabel.Text = text;
            DescriptionLabel.Visible = true;
        }
        else
        {
            DescriptionLabel.Visible = false;
        }
    }

    private void setType(InformationModalType type)
    {
        LoadingSpinner.Visible = false;
        ErrorIcon.Visible = false;
        CloseButton.Visible = false;

        switch (type)
        {
            case InformationModalType.Loading:
                LoadingSpinner.Visible = true;
                break;
            case InformationModalType.Error:
                ErrorIcon.Visible = true;
                CloseButton.Visible = true;
                break;
        }
    }
}
