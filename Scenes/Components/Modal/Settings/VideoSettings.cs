using System.Linq;
using Godot;

namespace GodotPeer2PeerSteamCSharp.Scenes.Components.Modal.Settings;

public partial class VideoSettings: Control
{
    private int MaxResoltionWidth { get; set; }
    private int MaxResolutionHeight { get; set; }
    private int ResolutionWidth { get; set; }
    private int ResolutionHeight { get; set; }
    private bool IsFullscreen { get; set; }
    
    private OptionButton _resolutionOptionButton;
    
    private Resolution[] _availableResolutions = new[]
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

    
    public override void _Ready()
    {
        initializeVideoSettings();
        SetUINodesToVariables();
        PopulateUINodes();
    }

    private void initializeVideoSettings()
    {
        SetResolutionSettings();
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
    
    private void SetUINodesToVariables()
    {
        _resolutionOptionButton = GetNode<OptionButton>("%Resolution");
        _resolutionOptionButton.Select(_availableResolutions
            .ToList()
            .FindIndex(res => res.Width == ResolutionWidth && res.Height == ResolutionHeight));
        
    }

    private void PopulateUINodes()
    {
        foreach (var resolution in _availableResolutions)
        {
            _resolutionOptionButton.AddItem(resolution.ToString());
        }
    }
    
    private void FilterResolutionsByMaxSize()
    {
        _availableResolutions = _availableResolutions
            .Where(res => res.Width <= MaxResoltionWidth && res.Height <= MaxResolutionHeight)
            .ToArray();
    }
    
    public class Resolution
    {
        public int Width { get; }
        public int Height { get; }
        public string AspectRatio { get; }

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
