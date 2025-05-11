using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GodotPeer2PeerSteamCSharp.Autoload.Types.Scene;

public partial class ModalManager : Node
{
    public Dictionary<ModalType, string> Modals { get; private set; }

    private Node currentModalInstance;
    private SceneTreeTimer modalMinTimeTimer;
    
    private readonly string modalSceneDirectory = "res://Scenes/Components/Modal";
    private string baseScenePath;
    private const float minimumTimeModal = 0.75f;

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
        if (IsModalShowing() || !TryGetModalScenePath(modalType, out var path))
            return;

        var modalScene = GD.Load<PackedScene>(path);
        var modalInstance = modalScene.Instantiate();

        DisplayModal(modalInstance);
    }

    private void ShowConfiguredModal(Node modalScene)
    {
        if (IsModalShowing())
            return;

        DisplayModal(modalScene);
    }

    private void DisplayModal(Node modalContent)
    {
        var modalBaseScene = GD.Load<PackedScene>(baseScenePath);
        var modalBaseInstance = modalBaseScene.Instantiate();

        var container = modalBaseInstance.GetNode<CenterContainer>("%ModalContainer");
        modalContent.Name = "Modal";
        container.AddChild(modalContent);

        GetTree().Root.AddChild(modalBaseInstance);
        currentModalInstance = modalBaseInstance;
        modalMinTimeTimer = GetTree().CreateTimer(minimumTimeModal);
    }

    public void RenderInformationModal(string HeaderName, InformationModalType type, string description = null)
    {
        if (IsModalShowing())
        {
            var existingModalScene = currentModalInstance.GetNode<CenterContainer>("%ModalContainer").GetChild(0);
            if (existingModalScene is InformationModal informationModal)
            {
                informationModal.UpdateModal(HeaderName, type, description);
                return;
            }
        }
        
        if (!TryGetModalScenePath(ModalType.Information, out var path)) return;
        
        var modalScene = GD.Load<PackedScene>(path);
        var modalSceneInstance = modalScene.Instantiate<InformationModal>();
        modalSceneInstance.prepareModal(HeaderName, type, description);
        ShowConfiguredModal(modalSceneInstance);
    }
    
    public async Task CloseModal()
    {
        if (modalMinTimeTimer != null && modalMinTimeTimer.TimeLeft > 0)
        {
            await ToSignal(modalMinTimeTimer, SceneTreeTimer.SignalName.Timeout);
        }

        EventBus.UI.OnCloseModal();
        modalMinTimeTimer = null;
    }

    private bool IsModalShowing()
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