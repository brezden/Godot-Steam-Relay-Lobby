using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Godot;

namespace GodotPeer2PeerSteamCSharp.Modules.Config.Video;

public partial class VideoSettings : Node
{
    public int MaxResoltionWidth { get; private set; }
    public int MaxResolutionHeight { get; private set; }
    public int ResolutionWidth { get; private set; }
    public int ResolutionHeight { get; private set; }
    public int PreviousWindowWidth { get; private set; }
    public int PreviousWindowHeight { get; private set; }
    public WindowModeOption WindowMode { get; private set; }
    
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

    public enum WindowModeOption
    {
        Window,
        Fullscreen,
        BorderlessWindow,
        BorderlessFullscreen
    }
    
    public VideoSettings()
    {
        initializeVideoSettings();
    }
    
    private void initializeVideoSettings()
    {
        SetResolution();
        SetWindowMode();
    }
    
    public void ApplySettings()
    {
        ApplyResolutionAndWindowModeSettings();
    }

    public void SetResolution(int? width = null, int? height = null)
    {
        Vector2I maxResolution = DisplayServer.ScreenGetSize();
        MaxResoltionWidth = maxResolution.X;
        MaxResolutionHeight = maxResolution.Y;
        
        if (width.HasValue && height.HasValue) 
        {
            ResolutionWidth = width.Value;
            ResolutionHeight = height.Value;
        }
        else
        {
            Vector2I currentResolution = DisplayServer.WindowGetSize();
            ResolutionWidth = currentResolution.X;
            ResolutionHeight = currentResolution.Y;
        }
        
        FilterResolutionsByMaxSize();
    }

    public void SetWindowMode(WindowModeOption? windowMode = null)
    {
        if (windowMode == null)
        {
            DisplayServer.WindowMode currentMode = DisplayServer.WindowGetMode();
            bool isBorderless = DisplayServer.WindowGetFlag(DisplayServer.WindowFlags.Borderless);
            
            if (currentMode == DisplayServer.WindowMode.Windowed)
            {
                WindowMode = isBorderless ? WindowModeOption.BorderlessWindow : WindowModeOption.Window;
            }
            else if (currentMode == DisplayServer.WindowMode.Fullscreen)
            {
                WindowMode = isBorderless ? WindowModeOption.BorderlessFullscreen : WindowModeOption.Fullscreen;
            }
            else
            {
                WindowMode = WindowModeOption.Window;
            }

            return;
        }
        
        Logger.Game($"Setting window mode to: {windowMode.Value}");
        
        WindowMode = windowMode.Value;
    }
    
    private void ApplyResolutionAndWindowModeSettings()
    {
        switch (WindowMode)
        {
            case WindowModeOption.Window:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
                DisplayServer.WindowSetSize(new Vector2I(ResolutionWidth, ResolutionHeight));
                CenterWindow();
                break;
            case WindowModeOption.Fullscreen:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
                break;
            case WindowModeOption.BorderlessWindow:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);
                DisplayServer.WindowSetSize(new Vector2I(ResolutionWidth, ResolutionHeight));
                CenterWindow();
                break;
            case WindowModeOption.BorderlessFullscreen:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);
                break;
            default:
                Logger.Error("Invalid window mode option.");
                break;
        }
    }

    // This method also solves the issue when the control nodes are not redrawn after changing the resolution.
    private void CenterWindow()
    {
        int screenId = DisplayServer.WindowGetCurrentScreen();
        Vector2I screenSize = DisplayServer.ScreenGetSize(screenId);
        Vector2I centeredPos = (screenSize - new Vector2I(ResolutionWidth, ResolutionHeight)) / 2;
        DisplayServer.WindowSetPosition(centeredPos);
    }
    
    public Resolution[] GetFilteredResolutions()
    {
        if (_filteredResolutions == null || _filteredResolutions.Length == 0)
        {
            return _availableResolutions;
        }
        
        return _filteredResolutions;
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
