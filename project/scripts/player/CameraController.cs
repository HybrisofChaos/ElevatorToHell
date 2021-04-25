using Godot;
using System;

public class CameraController : Camera2D
{
    Node2D player;

    public override void _Ready()
    {
        this.SetAsToplevel(true);
        player = GetParent<Node2D>();
    }

    public override void _Process(float delta)
    {
    }

    public override void _PhysicsProcess(float delta)
    {
        if (this.Position.x + 10 > player.Position.x && this.Position.x - 10 < player.Position.x && this.Position.y + 10 > player.Position.y && this.Position.y - 10 < player.Position.y)
        {
            //this.Position = player.Position;
        }
        else
        {
            Vector2 direciton = this.Position.DirectionTo(player.Position);
            this.Position += direciton * 400 * delta;
        }



        //this.Rotation = 0;
    }
}
