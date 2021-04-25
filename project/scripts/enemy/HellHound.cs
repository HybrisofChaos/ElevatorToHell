using Godot;
using System;
using System.Collections.Generic;

public class HellHound : Enemy
{
    [Export]
    public int maxJumpDistance = 250;

    [Export]
    public int minJumpDistance = 250;

    [Export]
    public float jumpSpeed = 350;

    [Export]
    public float jumpCooldown = 1.2f;

    [Export]
    public float jumpWindupTime = 0.3f;

    [Export]
    public int jumpDamage = 150;

    private AnimatedSprite sprite;

    private bool canJump = true;
    private bool isJumping = false;
    private Vector2 jumpDir = Vector2.Zero;
    private float distanceCoveredJumping = 0f;
    private bool followAltPath = false;
    private bool generatedAltPath = false;
    private Line2D line;

    public override void _Ready()
    {
        base._Ready();

        sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        line = GetTree().Root.GetNode<Line2D>("Main/Game/Line2D");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        float distanceToPlayer = this.Position.DistanceTo(this.player.Position);
        if (distanceToPlayer <= maxJumpDistance
            && distanceToPlayer >= minJumpDistance && !isJumping)
        {
            DoJump();
        }

        if(isFollowingPath){
            if(!this.sprite.IsPlaying()){
                this.sprite.Play();
            }
        }else{
            if(this.sprite.IsPlaying()){
                this.sprite.Stop();
            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (this.isJumping)
        {
            float distanceToCover = jumpSpeed * delta;
            distanceCoveredJumping += distanceToCover;

            if(distanceCoveredJumping > maxJumpDistance){
                float deltaDistance = distanceCoveredJumping - maxJumpDistance;
                this.Position += jumpDir * deltaDistance;
                
                this.isJumping = false;
                this.jumpDir = Vector2.Zero;
                distanceCoveredJumping = 0;
                ResetJump();
            }else{
                this.Position += jumpDir * distanceToCover;
            }
        }

    }

    private async void DoJump()
    {
        if (!canJump || isJumping) return;

        this.shouldFollowPath = false;
        sprite.Frame = 0;
        jumpDir = this.Position.DirectionTo(this.player.Position);
        LookAt(this.player.Position);
        await ToSignal(GetTree().CreateTimer(jumpWindupTime), "timeout");

        isJumping = true;
    }

    private async void ResetJump()
    {
        canJump = false;
        this.shouldFollowPath = true;
        this.followAltPath = true;
        await ToSignal(GetTree().CreateTimer(jumpCooldown), "timeout");
        canJump = true;
    }

    public void OnHitboxBodyEntered(Godot.Object body)
    {
        // Only do damage when jumping
        if(!isJumping) return;

        if (body is Player)
        {            
            IDamageable damageable = (IDamageable)body;
            damageable.ApplyDamage(this, jumpDamage);
            isJumping = false;
            jumpDir = Vector2.Zero;
            distanceCoveredJumping = 0;
        }
    }

    protected override void FindPath(){
        if(generatedAltPath) return;

        if(followAltPath){
            RandomNumberGenerator rng = new RandomNumberGenerator();
            rng.Randomize();
            int randomDistance = rng.RandiRange(30, 50);
            Vector2[] path = this.nav.GetSimplePath(this.Position, GetRandomPositionAroundPlayer(maxJumpDistance + randomDistance));
            this.currentPath = new List<Vector2>(path);
            line.Points = path;
            generatedAltPath = true;
            ResetAltPathFind();
        }else{
            base.FindPath();
        }
    }

    private async void ResetAltPathFind(){
        await ToSignal(GetTree().CreateTimer(3f), "timeout");
        this.followAltPath = false;
        generatedAltPath = false;
    }

    private Vector2 GetRandomPositionAroundPlayer(float distanceFromPlayer){
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        float x = rng.RandfRange(-1, 1);
        rng.Randomize();
        float y = rng.RandfRange(-1, 1);

        return new Vector2(x, y) * distanceFromPlayer + this.player.Position;
    }

    protected override bool AmITooCloseToPlayer()
    {
        return false;
    }
}
