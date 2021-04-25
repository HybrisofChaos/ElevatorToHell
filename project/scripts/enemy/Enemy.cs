using Godot;
using System;
using System.Collections.Generic;

public class Enemy : KinematicBody2D, IDamageable, IPushable
{
    [Export]
    public int maxHealth = 1000;
    [Export]
    public string name;
    [Export]
    public int xp;
    protected int currentHealth;

    [Export]
    public float pathTickInterval = 0.5f;

    [Export]
    public int moveSpeed = 250;

    [Export]
    public float minDistanceToPathTarget = 75;

    [Signal]
    public delegate void OnEnemyDeath(Node2D source);

    private Timer pathTicker;
    protected KinematicBody2D player;
    private Navigation2D nav;
    private List<Vector2> currentPath = new List<Vector2>();
    protected bool isFollowingPath = false;
    protected bool shouldFollowPath = true;

    public override void _Ready()
    {
        this.currentHealth = this.maxHealth;
        this.nav = GetTree().Root.GetNode<Navigation2D>("Main/Game/Navigation2D");
        this.player = GetTree().Root.GetNode<KinematicBody2D>("Main/Game/Player");

        pathTicker = new Timer();
        // Add timer as a child to properly dispose of it when the enemy dies.
        this.AddChild(pathTicker);
        pathTicker.WaitTime = pathTickInterval;
        pathTicker.Connect("timeout", this, nameof(FindPath));
        pathTicker.Start();
    }

    public override void _PhysicsProcess(float delta)
    {
        if (currentPath.Count != 0)
        {
            if (!AmITooCloseToPlayer())
            {
                FollowPath(this.moveSpeed * delta);
            }
        }
    }

    public void ApplyDamage(Node source, int damage)
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
        if (!shouldFollowPath)
        {
            isFollowingPath = false;
            return;
        }

        float distanceToNextPoint = this.Position.DistanceTo(currentPath[0]);
        if (distanceToNextPoint < 0.1f)
        {
            currentPath.RemoveAt(0);
        }

        if (currentPath.Count > 0)
        {
            Vector2 target = currentPath[0];
            distanceToNextPoint = this.Position.DistanceTo(target);

            if (distanceToWalk >= distanceToNextPoint)
            {
                this.Position = target;
                return;
            }

            BeforeContinuePath(target);
            MoveAndSlide(this.Position.DirectionTo(target) * moveSpeed);
            isFollowingPath = true;
        }
    }

    private bool AmITooCloseToPlayer()
    {
        return this.Position.DistanceTo(player.Position) <= minDistanceToPathTarget;
    }

    public virtual void Push(Vector2 direction, float force)
    {
        this.Position += direction * force;
    }

    protected virtual void BeforeContinuePath(Vector2 target){
        LookAt(target);
    }
}