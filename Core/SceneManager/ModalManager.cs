using Godot;
using System;
using System.Collections.Generic;
using GodotPeer2PeerSteamCSharp.Autoload.Types.Scene;

public partial class ModalManager : Node
{
    public Dictionary<ModalType, string> Modals { get; private set; }

    private Node currentModalInstance;
    private readonly string modalSceneDirectory = "res://Scenes/Components/Modal";
    private string baseScenePath;
    private const float minimumTimeModal = 5.0f;

    public override void _Ready()
    {
        baseScenePath = $"{modalSceneDirectory}/ModalBase.tscn";
        
        Modals = new Dictionary<ModalType, string>
        {
            { ModalType.Information, $"{modalSceneDirectory}/Information/Modal.tscn" },
            { ModalType.InvitePlayer, $"{modalSceneDirectory}/InviteMembers/Modal.tscn" }
        };
    }

    public void ShowModal(ModalType modalType)
    {
        if (IsModalShowing() || !TryGetModalScenePath(modalType, out var path)) return;
        
        PackedScene modalBaseScene = GD.Load<PackedScene>(baseScenePath);
        PackedScene modalScene = GD.Load<PackedScene>(path);

        Node modalBaseSceneInstance = modalBaseScene.Instantiate();
        Node modalSceneInstance = modalScene.Instantiate();
        
        CenterContainer modalSceneContainer = modalBaseSceneInstance.GetNode<CenterContainer>("%ModalContainer"); 
        modalSceneContainer.AddChild(modalSceneInstance);
        
        modalSceneInstance.Name = "Modal"; // Used for getting the modal instance later for tweening
        
        GetTree().Root.AddChild(modalBaseSceneInstance);
        currentModalInstance = modalBaseSceneInstance;
    }
    
    private void ShowConfiguredModal(Node modalScene)
    {
        if (IsModalShowing()) return;
        
        PackedScene modalBaseScene = GD.Load<PackedScene>(baseScenePath);
        
        Node modalBaseSceneInstance = modalBaseScene.Instantiate();
        
        CenterContainer modalSceneContainer = modalBaseSceneInstance.GetNode<CenterContainer>("%ModalContainer"); 
        modalSceneContainer.AddChild(modalScene);

        modalScene.Name = "Modal"; // Used for getting the modal instance later for tweening
        
        GetTree().Root.AddChild(modalBaseSceneInstance);
        currentModalInstance = modalBaseSceneInstance;
    }

    public async void showInformationModal(string HeaderName, InformationModalType type, string description = null)
    {
        if (!TryGetModalScenePath(ModalType.Information, out var path)) return;
        
        var modalScene = GD.Load<PackedScene>(path);
        var modalSceneInstance = modalScene.Instantiate<InformationModal>();
        modalSceneInstance.prepareModal(HeaderName, type, description);
        ShowConfiguredModal(modalSceneInstance);
    }
    
    public void CloseModal()
    {
        EventBus.UI.OnCloseModal();
    }

    public bool IsModalShowing()
    {
        return IsInstanceValid(currentModalInstance);
    }
    
    private bool TryGetModalScenePath(ModalType type, out string path)
    {
        if (!Modals.TryGetValue(type, out path))
        {
            Logger.Error($"Modal type '{type}' not found.");
            return false;
        }
        return true;
    }
}