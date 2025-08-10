using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Godot;

namespace GodotPeer2PeerSteamCSharp.Modules.Config.Game;

public partial class GameSettings : Node, Settings
{
    public bool SkipIntro { get; private set; }

    public GameSettings()
    {
        InitializeSettings();
        ApplySettings();
    }
    
    public void InitializeSettings()
    {
        SkipIntro = (bool)ConfigManager.Instance._configFile.GetValue("Game", "SkipIntro", false);
    }
    
    public void SetSkipIntro(bool skip)
    {
        SkipIntro = skip;
    }
    
    public void ApplySettings()
    {
        // Apply settings
    }
    
    public void SaveSettings()
    {
        ConfigManager.Instance._configFile.SetValue("Game", "SkipIntro", SkipIntro);
        Logger.Game("Game settings saved.");
    }
}