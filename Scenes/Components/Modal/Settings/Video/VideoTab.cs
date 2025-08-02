using System.Linq;
using System.Net;
using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Config;
using GodotPeer2PeerSteamCSharp.Modules.Config.Video;

namespace GodotPeer2PeerSteamCSharp.Scenes.Components.Modal.Settings;

public partial class VideoTab: Control
{
    private OptionButton _resolutionOptionButton;
    
    public override void _Ready()
    {
        BindUINodesToVariables();
        PopulateUINodes();
    }
    
    private void BindUINodesToVariables()
    {
        _resolutionOptionButton = GetNode<OptionButton>("%Resolution");
    }

    private void PopulateUINodes()
    {
        PopulateResolutionOptionButton();
    }
    
    private void PopulateResolutionOptionButton()
    {
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
        }
        
        _resolutionOptionButton.AddSeparator();
        
        foreach (var resolution in _filteredResolutions)
        {
            _resolutionOptionButton.AddItem(resolution.ToString());
        }
    }
}
