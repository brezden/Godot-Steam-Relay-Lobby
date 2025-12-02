using Godot;

public partial class Bullet : Area2D
{
    [Export] public float Speed = 100f;
    public Vector2 Direction = Vector2.Right;

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += Direction * Speed * (float)delta;
    }
}