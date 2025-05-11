using Godot;
using System;

public partial class InformationModal : Panel
{
	public string _headerText;
	
	public override void _Ready()
	{
		var _header = GetNode<Label>("%Header");
		_header.Text = $"{_headerText}...";
		
	}
}
