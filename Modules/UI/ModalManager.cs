using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class ModalManager : Node
{
    /** The minimum time a modal should be displayed */
    private const float minimumTimeModal = 0.75f;
    
    private static readonly string modalSceneDirectory = "res://Scenes/Components/Modal";
    private static string baseScenePath = $"{modalSceneDirectory}/ModalBase.tscn";

    private ModalBase modalBaseInstance;
    private CenterContainer modalBaseContainer;
    private Node currentModalInstance;
    
    private SceneTreeTimer modalMinTimeTimer;
    private CanvasLayer _overlayLayer;
    private Dictionary<ModalType, string> Modals = new Dictionary<ModalType, string>
    {
        { ModalType.Information, $"{modalSceneDirectory}/Information/Modal.tscn" },
        { ModalType.InvitePlayer, $"{modalSceneDirectory}/InviteMembers/Modal.tscn" },
        { ModalType.Settings, $"{modalSceneDirectory}/Settings/Modal.tscn" }
    };

    public override void _Ready()
    {
        _overlayLayer = GetTree().Root.GetNode<CanvasLayer>("Main/OverlayLayer");
        var modalBaseScene = GD.Load<PackedScene>(baseScenePath);
        modalBaseInstance = (ModalBase) modalBaseScene.Instantiate();
        modalBaseContainer = modalBaseInstance.GetNode<CenterContainer>("%ModalContainer");
    }
    
    /** Renders a modal of the specified type.
     * 
     * @param modalType The type of modal to render.
     */
    public void RenderModal(ModalType modalType)
    {
        if (!TryGetModalScenePath(modalType, out var path))
            return;

        var modalScene = GD.Load<PackedScene>(path);
        var modalInstance = modalScene.Instantiate();

        DisplayModal(modalInstance);
    }

    /** Renders an information modal with the specified header, type, and optional description.
     * If an information modal is already showing, it updates the existing modal instead of creating a new one.
     * 
     * @param HeaderName The title of the modal.
     * @param type The type of information modal to display.
     * @param description Optional description text for the modal.
     */
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

        if (!TryGetModalScenePath(ModalType.Information, out var path))
            return;

        var modalScene = GD.Load<PackedScene>(path);
        var modalSceneInstance = modalScene.Instantiate<InformationModal>();
        modalSceneInstance.prepareModal(HeaderName, type, description);
        DisplayModal(modalSceneInstance);
    }

    /** Closes the currently displayed modal.
     * If a minimum display time is set, it waits for that time before closing.
     */
    public async Task CloseModal()
    {
        if (modalMinTimeTimer != null && modalMinTimeTimer.TimeLeft > 0)
            await ToSignal(modalMinTimeTimer, SceneTreeTimer.SignalName.Timeout);
        
        modalMinTimeTimer = null;
        await modalBaseInstance.AnimationOut();
        modalBaseContainer.GetChild(0).QueueFree();
        _overlayLayer.RemoveChild(modalBaseInstance);
    }
    
    public bool IsModalShowing()
    {
        return IsInstanceValid(currentModalInstance);
    }
    
    private void DisplayModal(Node modalContent)
    {
        modalContent.Name = "Modal";
        modalBaseContainer.AddChild(modalContent);
        _overlayLayer.AddChild(modalBaseInstance);
        modalMinTimeTimer = GetTree().CreateTimer(minimumTimeModal);
        modalBaseInstance.AnimationIn();
    }

    private bool TryGetModalScenePath(ModalType type, out string path)
    {
        if (!Modals.TryGetValue(type, out path))
        {
            Logger.Error($"Modal type '{type}' not found");
            return false;
        }

        return true;
    }
}
