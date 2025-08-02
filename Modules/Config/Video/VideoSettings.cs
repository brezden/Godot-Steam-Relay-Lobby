using System.Linq;
using System.Net;
using Godot;

namespace GodotPeer2PeerSteamCSharp.Modules.Config.Video;

public class VideoSettings
{
    public int MaxResoltionWidth { get; private set; }
    public int MaxResolutionHeight { get; private set; }
    public int ResolutionWidth { get; private set; }
    public int ResolutionHeight { get; private set; }
    public bool IsFullscreen { get; private set; }
    
    private readonly Resolution[] _availableResolutions = new[]
    {
        new Resolution(640, 480, "4:3"), 
        new Resolution(800, 600, "4:3"),
        new Resolution(1024, 768, "4:3"),
        new Resolution(1280, 720, "16:9"),
        new Resolution(1280, 800, "16:10"),
        new Resolution(1280, 960, "4:3"),
        new Resolution(1366, 768, "16:9"),
        new Resolution(1440, 900, "16:10"),
        new Resolution(1600, 900, "16:9"), 
        new Resolution(1680, 1050, "16:10"),
        new Resolution(1920, 1080, "16:9"),
        new Resolution(1920, 1200, "16:10"), 
        new Resolution(2560, 1080, "21:9"),
        new Resolution(2560, 1440, "16:9"),
        new Resolution(2560, 1600, "16:10"),
        new Resolution(3440, 1440, "21:9"),
    };

    private Resolution[] _filteredResolutions; 
    
    public VideoSettings()
    {
        initializeVideoSettings();
    }
    
    private void initializeVideoSettings()
    {
        SetResolutionSettings();
        SetFullscreenSettings();
    }

    private void SetResolutionSettings()
    {
        Vector2I maxResolution = DisplayServer.ScreenGetSize();
        MaxResoltionWidth = maxResolution.X;
        MaxResolutionHeight = maxResolution.Y;
        
        Vector2I currentResolution = DisplayServer.WindowGetSize();
        ResolutionWidth = currentResolution.X;
        ResolutionHeight = currentResolution.Y;
        
        FilterResolutionsByMaxSize();
    }
    
    public Resolution[] GetFilteredResolutions()
    {
        SetResolutionSettings(); // For the edge case the user has changed their display settings
        if (_filteredResolutions == null || _filteredResolutions.Length == 0)
        {
            return _availableResolutions;
        }
        
        return _filteredResolutions;
    }
    
    private void SetFullscreenSettings()
    {
        IsFullscreen = DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen;
    }
    
    private void FilterResolutionsByMaxSize()
    {
        _filteredResolutions = _availableResolutions
            .Where(res => res.Width <= MaxResoltionWidth && res.Height <= MaxResolutionHeight)
            .ToArray();
    }
    
    public class Resolution
    {
        public int Width { get; }
        public int Height { get; }
        private string AspectRatio { get; }

        public Resolution(int width, int height, string aspectRatio)
        {
            Width = width;
            Height = height;
            AspectRatio = aspectRatio;
        }

        public override string ToString()
        {
            return $"{Width}x{Height} ({AspectRatio})";
        }
    }
}
