using System;
using System.Threading.Tasks;
using Godot;

public partial class ModalBase : Node
{
    private Panel _modalPanel;

    private ShaderMaterial _shaderMaterial;
    
    [Export] public float blurAmount = 1.75f;
    [Export] public float modalInDuration = 0.25f;
    [Export] public float modalOutDuration = 0.25f;

    public override void _EnterTree()
    {
        var shaderCanvasLayer = GetNode<CanvasLayer>("Blur");
        _shaderMaterial = (ShaderMaterial) shaderCanvasLayer.GetNode<ColorRect>("ColorRect").Material;
        _shaderMaterial.SetShaderParameter("lod", 0f);

        var modalContainer = shaderCanvasLayer.GetNode<CenterContainer>("ModalContainer");
        _modalPanel = modalContainer.GetNode<Panel>("Modal");
        _modalPanel.Modulate = new Color(1, 1, 1, 0.0f);
    }

    public async Task AnimationIn()
    {
        if (_shaderMaterial == null || _modalPanel == null)
        {
            return;
        }
        
        var shaderTween = CreateTween();
        shaderTween.TweenProperty(_shaderMaterial, "shader_parameter/lod", blurAmount, modalInDuration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);
        
        var modalTween = CreateTween();
        modalTween.TweenProperty(_modalPanel, "modulate", new Color(1, 1, 1), modalInDuration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.Out);

        await ToSignal(modalTween, Tween.SignalName.Finished);
        await ToSignal(shaderTween, Tween.SignalName.Finished);
    }

    public async Task AnimationOut()
    {
        if (_modalPanel == null || _shaderMaterial == null)
        {
            return;
        }
        
        var shaderTween = CreateTween();
        shaderTween.TweenProperty(_shaderMaterial, "shader_parameter/lod", 0, modalOutDuration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.In);

        var modalTween = CreateTween();
        modalTween.TweenProperty(_modalPanel, "modulate", new Color(1, 1, 1, 0.0f), modalOutDuration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.In);

        await ToSignal(shaderTween, Tween.SignalName.Finished);
        await ToSignal(modalTween, Tween.SignalName.Finished);
    }
}
