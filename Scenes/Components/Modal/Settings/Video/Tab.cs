using System;
using System.Linq;
using System.Net;
using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Config;
using GodotPeer2PeerSteamCSharp.Modules.Config.Video;

namespace GodotPeer2PeerSteamCSharp.Scenes.Components.Modal.Settings.Video;

public partial class Tab: Control
{
    private OptionButton _resolutionOptionButton;
    private OptionButton _windowModeOptionButton;
    
    public override void _Ready()
    {
        BindUINodesToVariables();
        PopulateUINodes();
    }
    
    private void BindUINodesToVariables()
    {
        _resolutionOptionButton = GetNode<OptionButton>("%Resolution");
        _resolutionOptionButton.ItemSelected += OnResolutionChanged;
        _windowModeOptionButton = GetNode<OptionButton>("%WindowMode");
        _windowModeOptionButton.ItemSelected += OnWindowModeChanged;
    }

    private void PopulateUINodes()
    {
        PopulateWindowMode();
        PopulateResolution();
    }
    
    private void PopulateWindowMode()
    {
        _windowModeOptionButton.Clear();
        
        foreach (VideoSettings.WindowModeOption windowMode in Enum.GetValues(typeof(VideoSettings.WindowModeOption)))
        {
            _windowModeOptionButton.AddItem(windowMode.ToString());
        }
        
        _windowModeOptionButton.Select((int)ConfigManager.Instance.VideoSettings.WindowMode);
    }

    private void OnResolutionChanged(long index)
    {
        if (index < 0 || index >= _resolutionOptionButton.GetItemCount())
        {
            Logger.Error($"Invalid resolution index selected. {index}");
            return;
        }
    
        // Get the selected resolution string
        string selectedResolutionText = _resolutionOptionButton.GetItemText((int)index);
    
        // Split the resolution string to extract width and height
        string[] resolutionParts = selectedResolutionText.Split('x');
        if (resolutionParts.Length < 2)
        {
            Logger.Error($"Invalid resolution format: {selectedResolutionText}");
            return;
        }

        // Parse width and height
        string widthText = resolutionParts[0].Trim();
        string heightText = resolutionParts[1].Split(' ')[0].Trim();

        if (int.TryParse(widthText, out int width) && int.TryParse(heightText, out int height))
        {
            ConfigManager.Instance.VideoSettings.SetResolution(width, height);
        }
        else
        {
            Logger.Error($"Failed to parse resolution: {selectedResolutionText}");
        }
    }
    
    private void OnWindowModeChanged(long index)
    {
        VideoSettings.WindowModeOption selectedWindowMode = (VideoSettings.WindowModeOption)index;
        ConfigManager.Instance.VideoSettings.SetWindowMode(selectedWindowMode);
        PopulateResolution();
    }
    
    private void PopulateResolution()
    {
        VideoSettings.WindowModeOption windowMode = ConfigManager.Instance.VideoSettings.WindowMode;
        _resolutionOptionButton.Disabled = false;
        _resolutionOptionButton.Clear();
        
        if (windowMode == VideoSettings.WindowModeOption.Fullscreen ||
            windowMode == VideoSettings.WindowModeOption.BorderlessFullscreen)
        {
            _resolutionOptionButton.AddItem($"{ConfigManager.Instance.VideoSettings.MaxResoltionWidth}x{ConfigManager.Instance.VideoSettings.MaxResolutionHeight} (Max)");
            _resolutionOptionButton.Select(0);
            _resolutionOptionButton.Disabled = true;
            return;
        }
        
        VideoSettings.Resolution[] _filteredResolutions = ConfigManager.Instance.VideoSettings.GetFilteredResolutions();
        
        // check if current resolution is in the filtered resolutions by checking in the array for the current resolution
        int currentResolutionIndex = _filteredResolutions
            .ToList()
            .FindIndex(resolution => 
                resolution.Width == ConfigManager.Instance.VideoSettings.ResolutionWidth && 
                resolution.Height == ConfigManager.Instance.VideoSettings.ResolutionHeight);
        
        if (currentResolutionIndex == -1)
        {
            Logger.Game($"Current resolution {ConfigManager.Instance.VideoSettings.ResolutionWidth}x{ConfigManager.Instance.VideoSettings.ResolutionHeight} not found in available resolutions.");
            _resolutionOptionButton.AddItem($"{ConfigManager.Instance.VideoSettings.ResolutionWidth}x{ConfigManager.Instance.VideoSettings.ResolutionHeight} (Custom");
        }
        else
        {
            _resolutionOptionButton.AddItem(_filteredResolutions[currentResolutionIndex].ToString());
            ConfigManager.Instance.VideoSettings.SetResolution(
                _filteredResolutions[currentResolutionIndex].Width, 
                _filteredResolutions[currentResolutionIndex].Height);
        }
        
        _resolutionOptionButton.AddSeparator();
        
        foreach (var resolution in _filteredResolutions)
        {
            _resolutionOptionButton.AddItem(resolution.ToString());
        }
    }
}
