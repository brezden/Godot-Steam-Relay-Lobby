using Godot;
using System;

public partial class ModalBase : Node
{
	[Export] public float blurAmount = 1.75f;
	[Export] public float blurInDuration = 0.25f;
	[Export] public float blurOutDuration = 0.25f;
	[Export] public float modalInDuration = 0.25f;
	[Export] public float modalOutDuration = 0.25f;

	private ShaderMaterial _shaderMaterial;
	private Panel _modalPanel;
	
	public override void _Ready()
	{
		EventBus.UI.CloseModal += AnimationOut;
		
		var shaderCanvasLayer = GetNode<CanvasLayer>("Blur");
		_shaderMaterial = (ShaderMaterial) shaderCanvasLayer.GetNode<ColorRect>("ColorRect").Material;
		_shaderMaterial.SetShaderParameter("lod", 0f);
		
		var modalContainer = shaderCanvasLayer.GetNode<CenterContainer>("ModalContainer");
		_modalPanel = modalContainer.GetNode<Panel>("Modal");
		_modalPanel.Modulate = new Color(1, 1, 1, 0.0f);
		AnimationIn();
	}

	public override void _ExitTree()
	{
		EventBus.UI.CloseModal -= AnimationOut;
	}

	public void AnimationIn()
	{
		BlurIn();
		ModalAnimationIn();
	}

	public void AnimationOut(object sender, EventArgs e)
	{
		BlurOut();
		ModalAnimationOut();
	}

	private void BlurIn()
	{
		var tween = CreateTween();
		tween.TweenProperty(_shaderMaterial, "shader_parameter/lod", blurAmount, blurInDuration)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.Out);
	}
	
	public void ModalAnimationIn()
	{
		var tween = CreateTween();
		tween.TweenProperty(_modalPanel, "modulate", new Color(1, 1, 1, 1.0f), modalInDuration)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.Out);
	}

	
	private void BlurOut()
	{
		var tween = CreateTween();
		tween.TweenProperty(_shaderMaterial, "shader_parameter/lod", 0, blurOutDuration)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.In);

		tween.TweenCallback(Callable.From(() => QueueFree()));
	}

	public void ModalAnimationOut()
	{
		var tween = CreateTween();
		tween.TweenProperty(_modalPanel, "modulate", new Color(1, 1, 1, 0.0f), modalOutDuration)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.In);

		tween.TweenCallback(Callable.From(() => QueueFree()));
	}
}
