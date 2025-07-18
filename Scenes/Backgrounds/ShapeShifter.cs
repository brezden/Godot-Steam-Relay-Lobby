using Godot;

public partial class ShapeShifter : Control
{
    [Export] public NodePath BackgroundColorRectPath;
    [Export] public NodePath TransitionLayerPath;
    
    [Export] public float TransitionSpeed = 1.0f;
    [Export] public float WaitTime = 1.0f;

    private ColorRect _backgroundColor;
    private ShapeTransition _transitionLayer;
    private AnimationPlayer _animPlayer;
    private readonly RandomNumberGenerator _rng = new();
    
    private int _colorIndex = 0;

    private readonly Color[] _backgroundColors = new Color[]
    {
        new Color(0.96f, 0.80f, 0.80f), // Soft Rose
        new Color(0.99f, 0.87f, 0.73f), // Warm Apricot
        new Color(0.98f, 0.95f, 0.75f), // Butter Yellow
        new Color(0.85f, 0.96f, 0.78f), // Pale Green
        new Color(0.75f, 0.95f, 0.89f), // Mint Cream
        new Color(0.76f, 0.89f, 0.98f), // Powder Blue
        new Color(0.83f, 0.83f, 0.98f), // Lavender Mist
        new Color(0.90f, 0.78f, 0.98f), // Soft Lilac
        new Color(0.98f, 0.80f, 0.90f), // Blush Pink
        new Color(0.94f, 0.94f, 0.94f), // Light Gray
        new Color(0.90f, 0.96f, 0.92f), // Pale Mint
        new Color(0.98f, 0.92f, 0.92f), // Cloud Pink
    };

    public override void _Ready()
    {
        _colorIndex = _rng.RandiRange(0, _backgroundColors.Length - 1);
        
        _backgroundColor = GetNode<ColorRect>(BackgroundColorRectPath);
        _transitionLayer = GetNode<ShapeTransition>(TransitionLayerPath);
        _animPlayer = _transitionLayer.GetNode<AnimationPlayer>("AnimationPlayer");
        
        _transitionLayer.SetSpeedScale(TransitionSpeed);
        
        _backgroundColor.Color = _backgroundColors[_colorIndex];
        _colorIndex = (_colorIndex + 1) % _backgroundColors.Length; 
        
        PlayTransitions();
    }

    private async void PlayTransitions()
    {
        while (IsInsideTree())
        {
            await ToSignal(GetTree().CreateTimer(WaitTime), "timeout");
            
            var nextColor = _backgroundColors[_colorIndex % _backgroundColors.Length];
            _colorIndex = (_colorIndex + 1) % _backgroundColors.Length;

            _transitionLayer.SetColor(nextColor);
            _transitionLayer.PlayOnce();
            await ToSignal(_animPlayer, "animation_finished");

            _backgroundColor.Color = nextColor;
        }
    }
}
