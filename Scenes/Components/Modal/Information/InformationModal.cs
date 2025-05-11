using Godot;
using System;
using GodotPeer2PeerSteamCSharp.Autoload.Types.Scene;

public partial class InformationModal : Panel
{
	public string headerText;
	public string descriptionText;
	public InformationModalType type;
	
	private Label HeaderLabel;
	private Label DescriptionLabel;
	private Control LoadingSpinner;
	private Control ErrorIcon;
	
	public override void _Ready()
	{
		HeaderLabel = GetNode<Label>("%Header");
		DescriptionLabel = GetNode<Label>("%Description");
		LoadingSpinner = GetNode<Control>("%LoadingIcon");
		ErrorIcon = GetNode<Control>("%ErrorIcon");
		
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
		
		switch (type)
		{
			case InformationModalType.Loading:
				LoadingSpinner.Visible = true;
				break;
			case InformationModalType.Error:
				ErrorIcon.Visible = true;
				break;
			default:
				break;
		}
	}
}
