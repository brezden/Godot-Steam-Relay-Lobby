using Godot;
using System;

public partial class LoadingSpinner : AnimatedSprite2D
{
    public override void _Ready()
    {
        Play("default");
    }
}
