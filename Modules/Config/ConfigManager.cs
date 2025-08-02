using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Config.Video;

namespace GodotPeer2PeerSteamCSharp.Modules.Config;

public partial class ConfigManager: Node
{
    public VideoSettings VideoSettings { get; private set; }
    
    public static ConfigManager Instance
    {
        get;
        private set;
    }

    public override void _Ready()
    {
        if (Instance != null)
        {
            QueueFree();
            return;
        }

        Instance = this;
        VideoSettings = new VideoSettings();
    }
}