using Godot;

public partial class Tank : CharacterBody2D
{
    [Export] public float Speed = 35f;
    [Export] public float Accel = 100f;
    [Export] public float Friction = 50f;
    [Export] public float TurnSpeed = 10f;

    [Export] public FacingDir Facing = FacingDir.Down;
    [Export] public float AimDeadzone = 0.12f;
    [Export] public float test = 0f;
   
    private Sprite2D _tankBottom;
    private CollisionShape2D _tankCollision;

    public override void _Ready()
    {
        _tankBottom = GetNode<Sprite2D>("%Bottom");
        _tankCollision = GetNode<CollisionShape2D>("%CollisionShape2D");
    }

    public void ProcessInput(Vector2 input, double delta)
    {
        Vector2 targetVel = input * Speed;

        if (input != Vector2.Zero)
            Velocity = Velocity.MoveToward(targetVel, Accel * (float)delta);
        else
            Velocity = Velocity.MoveToward(Vector2.Zero, Friction * (float)delta);

        MoveAndSlide();

        if (input.Length() > AimDeadzone)
        {
            float targetAngle = input.Angle() + FacingToOffset(Facing);
            float alpha = 1f - Mathf.Exp(-TurnSpeed * (float)delta);
            float rotationAngle = Mathf.LerpAngle(_tankBottom.Rotation, targetAngle, alpha);
            _tankBottom.Rotation = rotationAngle;
            _tankCollision.Rotation = rotationAngle;
        }
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
}
