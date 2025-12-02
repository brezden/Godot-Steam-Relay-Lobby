using Godot;
using GodotSteam;

public partial class Tank : CharacterBody2D
{
    [Export] public float Speed = 35f;
    [Export] public float Accel = 100f;
    [Export] public float Friction = 50f;

    [Export] public FacingDir Facing = FacingDir.Down;
    [Export] public float MoveDeadzone = 0.12f;
    [Export] public float StickDeadzone = 0.20f;

    [Export] public float FacingOffset = -Mathf.Pi / 2f; // for aim sprite art direction

    private Sprite2D _tankBottom;
    private CollisionShape2D _tankCollision;

    private Sprite2D _aimSprite;
    private Vector2 _lastAim = Vector2.Down;
    [Export] public PackedScene BulletScene;

    public override void _Ready()
    {
        _tankBottom = GetNode<Sprite2D>("%Bottom");
        _tankCollision = GetNode<CollisionShape2D>("%CollisionShape2D");

        _aimSprite = GetNodeOrNull<Sprite2D>("%Aim"); 
        if (_aimSprite != null)
            _aimSprite.GlobalRotation = FacingOffset;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 moveInput = Input.GetVector("move_left_0", "move_right_0", "move_up_0", "move_down_0");
        ProcessMovement(moveInput, delta);
        ProcessAim();

        if (Input.IsActionJustPressed("action_0"))
        {
            Shoot();
        }
    }

    private void ProcessMovement(Vector2 input, double delta)
    {
        Vector2 snappedDir = SnapToCardinal(input, MoveDeadzone);
        Vector2 targetVel = snappedDir * Speed;

        if (snappedDir != Vector2.Zero)
            Velocity = Velocity.MoveToward(targetVel, Accel * (float)delta);
        else
            Velocity = Velocity.MoveToward(Vector2.Zero, Friction * (float)delta);

        MoveAndSlide();

        if (snappedDir != Vector2.Zero)
        {
            float baseAngle = snappedDir.Angle();
            float rotationAngle = baseAngle + FacingToOffset(Facing);
            _tankBottom.Rotation = rotationAngle;
            _tankCollision.Rotation = rotationAngle;
        }
    }
    
    private void ProcessAim()
    {
        if (_aimSprite == null)
            return;

        Vector2 stick = Input.GetVector("aim_left_0", "aim_right_0", "aim_up_0", "aim_down_0");

        if (stick.Length() <= StickDeadzone)
        {
            stick = Input.GetVector(
                "second_move_left_0",
                "second_move_right_0",
                "second_move_up_0",
                "second_move_down_0"
            );
        }

        if (stick.Length() > StickDeadzone)
            _lastAim = stick.Normalized();

        if (_lastAim == Vector2.Zero)
            return;

        float baseAngle = _lastAim.Angle();
        float snappedAngle = SnapAngle4(baseAngle);

        _aimSprite.GlobalRotation = snappedAngle + FacingOffset;
    }

    private static Vector2 SnapToCardinal(Vector2 v, float deadzone)
    {
        if (v.Length() <= deadzone)
            return Vector2.Zero;

        if (Mathf.Abs(v.X) > Mathf.Abs(v.Y))
            return v.X > 0 ? Vector2.Right : Vector2.Left;
        
        return v.Y > 0 ? Vector2.Down : Vector2.Up;
    }

    private static float SnapAngle4(float angle)
    {
        float step = Mathf.Pi / 2f;
        return Mathf.Round(angle / step) * step;
    }

    public enum FacingDir
    {
        Right,
        Up,
        Left,
        Down
    }

    private static float FacingToOffset(FacingDir dir) => dir switch
    {
        FacingDir.Right => 0f,
        FacingDir.Up    => -Mathf.Pi / 2f,
        FacingDir.Left  =>  Mathf.Pi,
        FacingDir.Down  =>  Mathf.Pi / 2f,
        _ => 0f
    };

    private void Shoot()
    {
        if (BulletScene == null)
            return;

        Vector2 dir = _lastAim;
        if (dir == Vector2.Zero)
            dir = Vector2.Down;

        dir = SnapToCardinal(dir, 0.01f);
        if (dir == Vector2.Zero)
            return;

        var bullet = BulletScene.Instantiate<Bullet>();

        bullet.Direction = dir;
        bullet.GlobalPosition = GlobalPosition;
        GetParent().AddChild(bullet);
        
        GD.Print($"Tank:   {GlobalPosition}");
        GD.Print($"Bullet: {bullet.GlobalPosition}");
    }
}
