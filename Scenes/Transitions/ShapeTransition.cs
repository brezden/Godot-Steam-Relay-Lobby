using Godot;

public partial class ShapeTransition : Control
{
    [Export] public Color ShapeColor { get; set; } = new Color(1, 0, 0);
    [Export] public float SpeedScale { get; set; } = 1.0f;

    private AnimationPlayer _animPlayer;
    
    private ColorRect _colorRect;

    public override void _Ready()
    {
        _colorRect = GetNode<ColorRect>("ColorRect");
        _animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        
        SetColor(ShapeColor);
        SetSpeedScale(SpeedScale);
    }

    public void SetColor(Color color)
    {
        _colorRect.Color = color;
    }

    public void SetSpeedScale(float speedScale)
    {
        _animPlayer.SpeedScale = speedScale;
    }

    public void PlayOnce()
    {
        _animPlayer.Play("Animation");
    }
}
