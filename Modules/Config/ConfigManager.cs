using System.Threading.Tasks;
using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Config.Video;
using GodotPeer2PeerSteamCSharp.Modules.Config.Game;

namespace GodotPeer2PeerSteamCSharp.Modules.Config;

public partial class ConfigManager: Node
{
    public ConfigFile _configFile;
    
    public VideoSettings VideoSettings { get; private set; }
    public GameSettings GameSettings { get; private set; }
    public static ConfigManager Instance { get; private set; }

    public override void _Ready()
    {
        if (Instance != null)
        {
            QueueFree();
            return;
        }

        Instance = this;
        setupLoadConfigFile();
        VideoSettings = new VideoSettings();
        GameSettings = new GameSettings();
    }

    private void setupLoadConfigFile()
    {
        _configFile = new ConfigFile();
        Error err = _configFile.Load("user://config.cfg");

        if (err != Error.Ok)
        {
            Logger.Game("Config file not found, creating a new one.");
        }
        else
        {
            Logger.Game("Config file loaded successfully.");
        }
        
        var backend = DisplayServer.GetName();
        Logger.Game($"Display backend: {backend}");
    }
    
    public void ApplySettings()
    {
        VideoSettings.ApplySettings();
        GameSettings.ApplySettings();
        SaveSettings();
    }

    public void SaveSettings()
    {
        VideoSettings.SaveSettings();
        GameSettings.SaveSettings();
        _configFile.Save("user://config.cfg");
    }
}