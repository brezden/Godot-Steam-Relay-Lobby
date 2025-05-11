using Godot;
using System;
using System.Collections.Generic;
using GodotPeer2PeerSteamCSharp.Autoload.Types.Scene;

public partial class ModalManager : Node
{
    public Dictionary<ModalType, string> Modals { get; private set; }

    private readonly string ModalSceneDirectory = "res://Scenes/Components/Modal";
    private string BaseScenePath;

    public override void _Ready()
    {
        BaseScenePath = $"{ModalSceneDirectory}/ModalBase.tscn";
        
        Modals = new Dictionary<ModalType, string>
        {
            { ModalType.Information, $"{ModalSceneDirectory}/Information/Modal.tscn" },
            { ModalType.InvitePlayer, $"{ModalSceneDirectory}/InviteMembers/Modal.tscn" }
        };
    }
    
    public void OpenModal(ModalType modalType)
    {
        if (!Modals.TryGetValue(modalType, out var path))
        {
            Logger.Error($"Modal type {modalType} not found.");
            return;
        }
        
        PackedScene modalBaseScene = GD.Load<PackedScene>(BaseScenePath);
        PackedScene modalScene = GD.Load<PackedScene>(path);
        
        Node modalBaseSceneInstance = modalBaseScene.Instantiate();
        Node modalSceneInstance = modalScene.Instantiate();
        
        CenterContainer modalSceneContainer = modalBaseSceneInstance.GetNode<CenterContainer>("%ModalContainer"); 
        modalSceneContainer.AddChild(modalSceneInstance);
        
        GetTree().Root.AddChild(modalBaseSceneInstance);
    }
}