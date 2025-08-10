using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Config;

namespace GodotPeer2PeerSteamCSharp.Scenes.Components.Modal.Settings.Game;

public partial class Tab: Control
{
    private CheckButton _skipIntroCheckBox;
    
    public override void _Ready()
    {
        BindUINodesToVariables();
        PopulateUINodes();
    }
    
    private void BindUINodesToVariables()
    {
        _skipIntroCheckBox = GetNode<CheckButton>("%SkipIntro");
        _skipIntroCheckBox.Toggled += OnSkipIntroToggled; 
    }
    
    private void PopulateUINodes()
    {
        _skipIntroCheckBox.ButtonPressed = ConfigManager.Instance.GameSettings.SkipIntro;
    }
    
    private void OnSkipIntroToggled(bool isChecked)
    {
        ConfigManager.Instance.GameSettings.SetSkipIntro(isChecked);
    }
}
