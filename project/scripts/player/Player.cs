using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    public int speed = 200;

    private Vector2 velocity = new Vector2();

    private AnimatedSprite animatedSprite;

    public override void _Ready(){
        animatedSprite = (AnimatedSprite) FindNode("PlayerSprite");
    }

    public void GetInput(){
        velocity = new Vector2();

        if(Input.IsActionPressed("left")){
            velocity.x -= 1;
        }
        if(Input.IsActionPressed("right")){
            velocity.x += 1;
        }
        if(Input.IsActionPressed("up")){
            velocity.y -= 1;
        }
        if(Input.IsActionPressed("down")){
            velocity.y += 1;
        }

        velocity = velocity.Normalized() * speed;
    }

    public override void _PhysicsProcess(float delta)
    {
        GetInput();
        LookAt(GetGlobalMousePosition());
        velocity = MoveAndSlide(velocity, Vector2.Up);

        if(animatedSprite != null){
            if(velocity != Vector2.Zero){
                animatedSprite.Play();
            }else{
                animatedSprite.Stop();
                animatedSprite.Frame = 0;
            }
        }
    
    }
}
