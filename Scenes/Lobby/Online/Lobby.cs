using Godot;
using System;
using GodotPeer2PeerSteamCSharp.Modules.Background;

public partial class Lobby : Control
{
	public override void _Ready()
	{
        BackgroundManager.Instance.LoadShapeTransform();
	}
}
