using Godot;
using System;

public partial class Modal : Panel
{
	private Label _header;
	
	public override void _Ready()
	{
		_header = GetNode<Label>("%Header");
	}
	
	public void Setup(string header)
	{
		if (_header == null)
		{
			Logger.Error("Header node not found in Modal.");
			return;
		}
		
		_header.Text = header;
	}
}
