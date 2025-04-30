using Godot;
using System;

public partial class ModalBase : Node
{
	[Export] public float blurAmount = 1.75f;
	[Export] public float blurInDuration = 1.5f;
	[Export] public float blurOutDuration = 1.25f;

	private ShaderMaterial _shaderMaterial;
	
	public override void _Ready()
	{
		var shaderCanvasLayer = GetNode<CanvasLayer>("Blur");
		_shaderMaterial = (ShaderMaterial) shaderCanvasLayer.GetNode<ColorRect>("ColorRect").Material;
		_shaderMaterial.SetShaderParameter("lod", 0f);
		BlurIn();
	}

	private void BlurIn()
	{
		var tween = CreateTween();
		tween.TweenProperty(_shaderMaterial, "shader_parameter/lod", blurAmount, blurInDuration)
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
}
