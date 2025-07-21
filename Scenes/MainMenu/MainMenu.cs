using Godot;
using System;
using GodotPeer2PeerSteamCSharp.Modules.Background;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
        BackgroundManager.Instance.LoadShapeTransform();
	}
}
