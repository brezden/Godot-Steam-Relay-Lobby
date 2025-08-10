using Godot;
using System;
using System.Collections.Generic;
using GodotPeer2PeerSteamCSharp.Modules.Config;

public partial class Modal : Panel
{
    private Button _confirmButton;
    private Button _closeButton;
    
    private TabBar _tabBar;
    
    private static int _currentTab;
    private static Control _videoTab;
    private static Control _gameTab;

    private Dictionary<int, Control> _tabs; 

    public override void _EnterTree()
    {
        _confirmButton = GetNode<Button>("%Confirm");
        _closeButton = GetNode<Button>("%Close");
        _tabBar = GetNode<TabBar>("%TabBar");

        _confirmButton.Pressed += OnConfirmButtonPressed;
        _closeButton.Pressed += OnCloseButtonPressed;

        _videoTab = GetNode<Control>("%VideoTab");
        _gameTab = GetNode<Control>("%GameTab");

        _tabs = new Dictionary<int, Control>
        {
            { 0, _videoTab },
            { 1, _gameTab }
        };

        foreach (var tab in _tabs.Values)
        {
            tab.Visible = false;
        }
        
        _tabs[0].Visible = true;

        _tabBar.TabSelected += OnTabSelected;
    }
    
    private void OnTabSelected(long index)
    {
        _tabs[_currentTab].Visible = false;
        
        if (_tabs.ContainsKey((int)index))
        {
            _currentTab = (int)index;
            _tabs[_currentTab].Visible = true;
        }
        else
        {
            Logger.Error($"Tab with index {index} does not exist.");
        }
    }
    
    private void OnConfirmButtonPressed()
    {
        ConfigManager.Instance.ApplySettings();
    }
    
    private void OnCloseButtonPressed()
    {
        UIManager.Instance.ModalManager.CloseModal();
    }
}