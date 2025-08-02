using Godot;
using System;
using System.Collections.Generic;
using GodotPeer2PeerSteamCSharp.Scenes.Components.Modal.Settings;

public partial class Modal : Panel
{
    private Button _confirmButton;
    
    private VideoSettings _videoSettings;

    public override void _Ready()
    {
        _confirmButton = GetNode<Button>("%Confirm");
        _confirmButton.Pressed += SaveSettings;
        
        _videoSettings = GetNode<VideoSettings>("%VideoSettings");
    }

    private void SaveSettings()
    {
        var config = new ConfigFile();
        
        _videoSettings.SaveSettings(config);
        
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
