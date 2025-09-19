using Godot;

public partial class Aim : Sprite2D
{
    [Export] public float StickDeadzone = 0.20f;
    [Export] public float TurnSpeed = 5f;
    [Export] public float FacingOffset = -Mathf.Pi / 2f;

    private Vector2 _lastAim = Vector2.Down;

    public override void _Ready()
    {
        GlobalRotation = FacingOffset;
    }

    public override void _Process(double delta)
    {
        Vector2 stick = Input.GetVector("aim_left_0","aim_right_0","aim_up_0","aim_down_0");

        if (stick.Length() > StickDeadzone)
            _lastAim = stick.Normalized();

        float target = _lastAim.Angle() + FacingOffset;
        float alpha = 1f - Mathf.Exp(-TurnSpeed * (float)delta);
        GlobalRotation = Mathf.LerpAngle(GlobalRotation, target, alpha);
    }
}