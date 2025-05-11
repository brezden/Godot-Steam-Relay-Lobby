using Godot;
using System;

public partial class ModalManager : Node
{
    public void OpenModal(int modalId = 0)
    {
        PackedScene modalBaseScene = GD.Load<PackedScene>("res://Scenes/Components/Modal/ModalBase.tscn");
        Node modelBaseSceneInstance = modalBaseScene.Instantiate();
        GetTree().Root.AddChild(modelBaseSceneInstance);
    }
}