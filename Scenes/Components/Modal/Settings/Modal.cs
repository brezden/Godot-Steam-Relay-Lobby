using Godot;
using System;
using System.Collections.Generic;
using GodotPeer2PeerSteamCSharp.Modules.Config;
using GodotPeer2PeerSteamCSharp.Modules.Config.Video;
using GodotPeer2PeerSteamCSharp.Scenes.Components.Modal.Settings;

public partial class Modal : Panel
{
    private Button _confirmButton;
    private Button _closeButton;
    private VideoTab _videoTab;

    public override void _Ready()
    {
        _confirmButton = GetNode<Button>("%Confirm");
        _closeButton = GetNode<Button>("%Close");

        _confirmButton.Pressed += OnConfirmButtonPressed;
        _closeButton.Pressed += OnCloseButtonPressed;
    }
    
    private void OnConfirmButtonPressed()
    {
        ConfigManager.Instance.ApplySettings();
    }
    
    private void OnCloseButtonPressed()
    {
        UIManager.Instance.ModalManager.CloseModal();
    }
}