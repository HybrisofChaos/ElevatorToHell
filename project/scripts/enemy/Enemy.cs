using Godot;
using System;
using System.Collections.Generic;

public class Enemy : KinematicBody2D, IDamageable
{
    [Export]
    public int maxHealth = 1000;
    protected int currentHealth;

    [Export]
    public float pathTickInterval = 0.5f;

    [Export]
    public int moveSpeed = 250;

    [Signal]
    public delegate void OnEnemyDeath(Enemy enemy);

    private Timer pathTicker;
    private KinematicBody2D player;
    private Navigation2D nav;
    private List<Vector2> currentPath = new List<Vector2>();

    public override void _Ready()
    {
        this.currentHealth = this.maxHealth;
        this.nav = GetTree().Root.GetNode<Navigation2D>("Main/Navigation2D");
        this.player = GetTree().Root.GetNode<KinematicBody2D>("Main/Player");

        pathTicker = new Timer();
        pathTicker.WaitTime = pathTickInterval;
        pathTicker.Connect("timeout", this, nameof(FindPath));
        pathTicker.Start();

        // Add timer as a child to properly dispose of it when the enemy dies.
        this.AddChild(pathTicker);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (currentPath.Count != 0)
        {
            FollowPath(this.moveSpeed * delta);
        }
    }

    public void ApplyDamage(int damage)
    {
        this.currentHealth -= damage;
        if (this.currentHealth <= 0)
        {
            EmitSignal(nameof(OnEnemyDeath), this);
            QueueFree();
        }
    }

    protected void FindPath()
    {
        Vector2[] path = nav.GetSimplePath(this.Position, player.Position);
        currentPath = new List<Vector2>(path);
    }

    protected void FollowPath(float distanceToWalk)
    {
        float distanceToNextPoint = this.Position.DistanceTo(currentPath[0]);
        if (distanceToNextPoint < 0.1f)
        {
            currentPath.RemoveAt(0);
        }

        if(currentPath.Count > 0){
            Vector2 target = currentPath[0];
            distanceToNextPoint = this.Position.DistanceTo(target);

            if(distanceToWalk >= distanceToNextPoint){
                this.Position = target;
                return;
            }

            LookAt(target);
            this.Position += this.Position.DirectionTo(target) * distanceToWalk;
        }
    }
}
