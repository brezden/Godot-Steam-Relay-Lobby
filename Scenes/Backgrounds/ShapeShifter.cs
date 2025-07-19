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
        new Color(0.15f, 0.15f, 0.15f), // Dark charcoal
        new Color(0.18f, 0.18f, 0.20f), // Graphite gray
        new Color(0.22f, 0.23f, 0.25f), // Slate gray
        new Color(0.18f, 0.22f, 0.24f), // Deep blue-gray
        new Color(0.20f, 0.20f, 0.24f), // Midnight gray-blue
        new Color(0.24f, 0.21f, 0.24f), // Desaturated eggplant
        new Color(0.25f, 0.22f, 0.20f), // Warm dark taupe
        new Color(0.20f, 0.24f, 0.22f), // Muted forest gray
        new Color(0.17f, 0.20f, 0.20f), // Cool gray-green
        new Color(0.16f, 0.18f, 0.21f), // Steel navy
        new Color(0.13f, 0.14f, 0.16f), // Dim night
    };

    public override void _Ready()
    {
        _colorIndex = 0; 
        
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
