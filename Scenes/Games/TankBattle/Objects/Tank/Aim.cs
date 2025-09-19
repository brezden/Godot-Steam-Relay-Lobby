using Godot;

public partial class Aim : Sprite2D
{
    [Export] public float StickDeadzone = 0.20f;
    [Export] public float TurnSpeed = 5f;
    [Export] public float FacingOffset = -Mathf.Pi / 2f; // down
    [Export] public float SourceHoldSeconds = 0.6f;
    [Export] public bool InvertAngles = false;

    private enum Source { None, Stick, Mouse }
    private Source _src = Source.None;

    private double _lastStickT = -999, _lastMouseT = -999;
    private Vector2 _lastAim = Vector2.Down;
    private Vector2 _lastScreenMouse;

    public override void _Ready()
    {
        GlobalRotation = FacingOffset;
        _lastScreenMouse = GetViewport().GetMousePosition();
    }

    public override void _Process(double delta)
    {
        double now = Time.GetTicksMsec() / 1000.0;

        // Stick input
        Vector2 stick = Input.GetVector("aim_left_0","aim_right_0","aim_up_0","aim_down_0");
        bool stickActive = stick.Length() > StickDeadzone;

        // Mouse activity (screen-space so camera motion doesn't count)
        Vector2 screenMouse = GetViewport().GetMousePosition();
        bool mouseMoved = (screenMouse - _lastScreenMouse).LengthSquared() > 0.5f; // tiny threshold
        bool mouseBtn = Input.IsMouseButtonPressed(MouseButton.Left) ||
                        Input.IsMouseButtonPressed(MouseButton.Right) ||
                        Input.IsMouseButtonPressed(MouseButton.Middle);

        if (stickActive)
        {
            _src = Source.Stick;
            _lastStickT = now;
            _lastScreenMouse = screenMouse;
        }
        else if ((mouseMoved || mouseBtn) && _src != Source.Stick)
        {
            _src = Source.Mouse;
            _lastMouseT = now;
            _lastScreenMouse = screenMouse;
        }
        else
        {
            if (_src == Source.Mouse && now - _lastMouseT > SourceHoldSeconds) _src = Source.None;
            if (_src == Source.Stick && now - _lastStickT > SourceHoldSeconds) _src = Source.None;
        }

        Vector2 aimDir = _lastAim;
        switch (_src)
        {
            case Source.Stick:
                aimDir = stick.Normalized();
                break;
            case Source.Mouse:
                var v = GetGlobalMousePosition() - GlobalPosition; // aim in world
                if (v.LengthSquared() > 0.000001f) aimDir = v.Normalized();
                break;
        }

        float ang = aimDir.Angle();
        if (InvertAngles) ang = -ang;

        float target = ang + FacingOffset;
        float alpha = 1f - Mathf.Exp(-TurnSpeed * (float)delta);
        GlobalRotation = Mathf.LerpAngle(GlobalRotation, target, alpha);

        _lastAim = aimDir;
        _lastScreenMouse = screenMouse;
    }
}
