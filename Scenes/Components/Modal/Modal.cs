using Godot;
using System;

public partial class Modal : Node
{
	[Export] public float blurAmount = 1.75f;

	private ShaderMaterial _shaderMaterial;
	
	public override void _Ready()
	{
		var shaderCanvasLayer = GetNode<CanvasLayer>("Blur");
		_shaderMaterial = (ShaderMaterial) shaderCanvasLayer.GetNode<ColorRect>("colorRect").Material;

		BlurIn();
	}

	private void BlurIn()
	{
		var tween = CreateTween();
		tween.TweenProperty(_shaderMaterial, "shader_parameter/LOD", blurAmount, 1)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.Out);
	}
	
	private void BlurOut()
	{
		var tween = CreateTween();
		tween.TweenProperty(_shaderMaterial, "shader_parameter/LOD", 0, 1)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.In);

		tween.TweenCallback(Callable.From(() => QueueFree()));
	}
}
