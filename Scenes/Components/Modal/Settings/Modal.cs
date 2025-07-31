using Godot;
using System;
using System.Collections.Generic;

public partial class Modal : Panel
{
    private Button _confirmButton;
    private OptionButton _resolutionDropdown;

    private int _userResolutionWidth;
    private int _userResolutionHeight;
    
    private const int DEFAULT_WINDOW_WIDTH = 1280;
    private const int DEFAULT_WINDOW_HEIGHT = 720;

    private Dictionary<string, Resolution> _availableResolutions = new() {};

    public override void _Ready()
    {
        _confirmButton = GetNode<Button>("%Confirm");
        _confirmButton.Pressed += OnConfirmButtonPressed;
        _resolutionDropdown = GetNode<OptionButton>("%Resolution");
        
        IntializeResolutionSettings();

        foreach (var res in _availableResolutions)
        {
            _resolutionDropdown.AddItem($"{res.Width}x{res.Height}");
        }
    }

    private void OnConfirmButtonPressed()
    {
        var selected = _resolutionDropdown.GetSelectedId();
        var res = _availableResolutions[selected];
        DisplayServer.WindowSetSize(new Vector2I(res.Width, res.Height));
    }

    private void IntializeResolutions()
    {
        PopulateFullscreenSettings();
        
        
        
        GetUsersResolution();
        
        var modes = DisplayServer.
    }
    
    private void PopulateFullscreenSettings()
    {
        // TODO: GET USERS WINDOW HEIGHT and ASCEPT RATIO
        // FILTER RESOLUTIONS BASED ON THAT
        // SHOW FULLSCREEN AND ASPECT RATIO OPTIONS
        // POPULATE THOSE BASED ON THE FILTERD RESOLUTIONS
    }
    
    private Resolution getAvailableResolutions(int index)
    {
        return _availableResolutions[index];
    }
    
    public class Resolution
    {
        public int Width { get; }
        public int Height { get; }

        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
