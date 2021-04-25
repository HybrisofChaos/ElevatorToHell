using Godot;
using System;

public class Player : KinematicBody2D, IDamageable
{
    [Export]
    public int maxHealth = 2000;
    private int currentHealth;

    [Export]
    public int speed = 200;

    [Export]
    public float shootCooldown = 1.5f;

    [Export]
    public PackedScene bullet = GD.Load<PackedScene>("res://scenes/projectile/Bullet.tscn");

    [Signal]
    public delegate void OnPlayerDeath(Node killer);

    private Vector2 velocity = new Vector2();

    private AnimationPlayer animationPlayer;
    private Position2D bulletSpawn;
    private bool canShoot = true;

    public override void _Ready()
    {
        this.currentHealth = maxHealth;

        animationPlayer = (AnimationPlayer)((Sprite)FindNode("PlayerSprite")).FindNode("AnimationPlayer");
    }

    public void GetInput()
    {
        velocity = new Vector2();

        if (Input.IsActionPressed("left"))
        {
            velocity.x -= 1;
        }
        if (Input.IsActionPressed("right"))
        {
            velocity.x += 1;
        }
        if (Input.IsActionPressed("up"))
        {
            velocity.y -= 1;
        }
        if (Input.IsActionPressed("down"))
        {
            velocity.y += 1;
        }

        velocity = velocity.Normalized() * speed;
    }

    public override void _PhysicsProcess(float delta)
    {
        GetInput();
        LookAt(GetGlobalMousePosition());
        velocity = MoveAndSlide(velocity, Vector2.Up);

        if (animationPlayer != null)
        {
            if (velocity != Vector2.Zero)
            {
                animationPlayer.Play("walk");
            }
            else
            {
                animationPlayer.Stop();
            }
        }
    }

    public void ApplyDamage(Node source, int damage)
    {
        currentHealth -= damage;
        GD.Print("Yikes, got hit by " + source.Name);
        if (currentHealth <= 0)
        {
            Die(source);
        }

    }

    private void Die(Node source)
    {
        GD.Print("Died by the hands of " + source.Name);
        EmitSignal(nameof(OnPlayerDeath), source);

        // QueueFree();
    }
}
