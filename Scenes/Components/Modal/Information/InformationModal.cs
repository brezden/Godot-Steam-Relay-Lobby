using Godot;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class InformationModal : Panel
{
    private TextureButton CloseButton;
    private Label DescriptionLabel;
    public string descriptionText;
    private Control ErrorIcon;
    private int errorCode;

    private Label HeaderLabel;
    public string headerText;
    private Control LoadingSpinner;
    public InformationModalType type;
    private Label ErrorCode; 

    public override void _Ready()
    {
        HeaderLabel = GetNode<Label>("%Header");
        DescriptionLabel = GetNode<Label>("%Description");
        LoadingSpinner = GetNode<Control>("%LoadingIcon");
        ErrorIcon = GetNode<Control>("%ErrorIcon");
        CloseButton = GetNode<TextureButton>("%CloseButton");
        ErrorCode = GetNode<Label>("%ErrorCode");

        setHeaderText(headerText);
        setDescriptionText(descriptionText);
        setType(type);
        
        if (errorCode != -1)
        {
            setErrorCode(errorCode);
        }
    }

    public void prepareModal(string header, InformationModalType type, string description = null, int errorCode = -1)
    {
        headerText = header;
        this.type = type;
        descriptionText = description;
        this.errorCode = errorCode;
    }

    public void UpdateModal(string header, InformationModalType type, string description = null, int errorCode = -1)
    {
        headerText = header;
        this.type = type;
        descriptionText = description;
        this.errorCode = errorCode;

        setHeaderText(headerText);
        setType(type);
        setDescriptionText(descriptionText);
        setErrorCode(errorCode);
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
        ErrorCode.Visible = false;

        switch (type)
        {
            case InformationModalType.Loading:
                LoadingSpinner.Visible = true;
                break;
            case InformationModalType.Error:
                ErrorIcon.Visible = true;
                CloseButton.Visible = true;
                ErrorCode.Visible = true;
                break;
        }
    }
    
    private void setErrorCode(int errorCode)
    {
        if (errorCode == -1)
        {
            ErrorCode.Visible = false;
            return;
        }
        
        this.errorCode = errorCode;
        ErrorCode.Text = $"Error Code: {errorCode}";
    }
}
