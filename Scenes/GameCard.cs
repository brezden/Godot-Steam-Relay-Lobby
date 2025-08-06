using Godot;
using System;

public partial class GameCard : MarginContainer
{
    [Export(PropertyHint.Range, "0,3,1")]
    private int cardType = 0;

    private TextureRect frame;
    private Material glare;
    private AnimationPlayer animationPlayer;
    private ShaderMaterial perspectiveShaderMaterial;
    private bool isHovered = false;
    private float time = 0f;
    private Tween tween;

    public override void _Ready()
    {
        frame = GetNode<TextureRect>("%Frame");
        var colorRect = GetNode<ColorRect>("%Glare");
        animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
        
        glare = colorRect.Material.Duplicate() as ShaderMaterial;
        colorRect.Material = glare;
        
        var perspectiveShader = GetNode<TextureRect>("%PerspectiveShader");
        perspectiveShaderMaterial = perspectiveShader.Material.Duplicate() as ShaderMaterial;
        perspectiveShader.Material = perspectiveShaderMaterial;


        MouseEntered += OnGameCardEntered;
        FocusEntered += OnGameCardEntered;
        MouseExited += OnGameCardExited;
        FocusExited += OnGameCardExited;
        SetProcess(false);

        UpdateFrameTexture();
        OnGameCardExited();
    }
    
    public override void _Process(double delta)
    {
        time += (float)delta;

        float x = Mathf.Sin(time * 2.2f) * 2f; // horizontal wobble
        float y = Mathf.Cos(time * 1.8f) * 2f; // vertical wobble

        perspectiveShaderMaterial.SetShaderParameter("x_rot", x);
        perspectiveShaderMaterial.SetShaderParameter("y_rot", y);
    }

    private void OnGameCardEntered()
    {
        if (isHovered) return;
        SetProcess(true);
        isHovered = true;
        
        animationPlayer.Play("main");
        
        if (glare is ShaderMaterial shaderMaterial)
        {
            shaderMaterial.SetShaderParameter("Position", 0f);
            shaderMaterial.SetShaderParameter("Speed", 0.2f);
        }
    }

    private void OnGameCardExited()
    {
        if (!isHovered) return;
        SetProcess(false);
        isHovered = false;
        
        animationPlayer.Play("exit");
        
        if (glare is ShaderMaterial shaderMaterial)
        {
            shaderMaterial.SetShaderParameter("Position", 0f);
            shaderMaterial.SetShaderParameter("Speed", 0f);
        }
        float currentX = (float)perspectiveShaderMaterial.GetShaderParameter("x_rot");
        float currentY = (float)perspectiveShaderMaterial.GetShaderParameter("y_rot");
        
        tween?.Kill(); 
        tween = GetTree().CreateTween();
        tween.TweenMethod(
            Callable.From<float>(x => perspectiveShaderMaterial.SetShaderParameter("x_rot", x)),
            currentX, 0f, 0.2f
        ).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
        tween.SetParallel().TweenMethod(
            Callable.From<float>(y => perspectiveShaderMaterial.SetShaderParameter("y_rot", y)),
            currentY, 0f, 0.2f
        ).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
    }

    private void UpdateFrameTexture()
    {
        if (frame.Texture is AtlasTexture sharedAtlas)
        {
            // Duplicate the AtlasTexture so each GameCard gets its own instance
            var atlasCopy = (AtlasTexture)sharedAtlas.Duplicate();
            Rect2 region = atlasCopy.Region;
            region.Position = new Vector2(cardType * 72, region.Position.Y);
            atlasCopy.Region = region;

            frame.Texture = atlasCopy;
        }
        else
        {
            Logger.Error("Frame texture is not an AtlasTexture.");
        }
    }
}