using Godot;
using System;
using System.Collections.Generic;
using GodotPeer2PeerSteamCSharp.Modules.Config.Video;
using GodotPeer2PeerSteamCSharp.Scenes.Components.Modal.Settings;

public partial class Modal : Panel
{
    private Button _confirmButton;
    
    private VideoTab _VideoTab;

    public override void _Ready()
    {
        _confirmButton = GetNode<Button>("%Confirm");
        _confirmButton.Pressed += SaveSettings;
        
        _VideoTab = GetNode<VideoTab>("%VideoTab");
    }

    private void SaveSettings()
    {
        var config = new ConfigFile();
        
        Error errorResult = config.Save("user://settings.cfg");
        
        if (errorResult != Error.Ok)
        {
            Logger.Error($"Failed to save settings: {errorResult}");
        }
        else
        {
            Logger.Game("Settings saved successfully.");
        }
    }
}
