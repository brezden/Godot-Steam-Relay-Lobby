using Godot;

public partial class CloseModalButton : Button
{
    public override void _Ready()
    {
        Pressed += CloseModal;
    }

    private void CloseModal()
    {
        EventBus.UI.OnCloseModal();
    }
}
