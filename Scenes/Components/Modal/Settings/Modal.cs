using Godot;
using System;
using System.Collections.Generic;
using GodotPeer2PeerSteamCSharp.Modules.Config;
using GodotPeer2PeerSteamCSharp.Modules.Config.Video;
using GodotPeer2PeerSteamCSharp.Scenes.Components.Modal.Settings;

public partial class Modal : Panel
{
    private Button _confirmButton;
    private VideoTab _videoTab;

    public override void _Ready()
    {
        _confirmButton = GetNode<Button>("%Confirm");
        _confirmButton.Pressed += () =>
        {
            ConfigManager.Instance.ApplySettings();
        };
    }
}