using Godot;
using System;
using System.Diagnostics;

public class WeaponController : Node
{
    [Export]
    public float timeBetweenCombos = 0.4f;

    [Export]
    public int lightAttackDamage = 100;

    [Export]
    public float lightAttackPushingForce = 100f;

    [Export]
    public float shootCooldown = 1.5f;

    [Export]
    public float selfShotgunForce = 30;

    private bool canAttack = true;
    private bool canShoot = true;

    private AnimatedSprite playerBody;
    private KinematicBody2D player;

    // When implementing heavy attack we need two hitboxes.
    private CollisionShape2D hitbox;

    private Position2D bulletSpawn;
    private PackedScene bullet = GD.Load<PackedScene>("res://scenes/projectile/Bullet.tscn");

    public override void _Ready()
    {
        this.playerBody = (AnimatedSprite)GetNode("../PlayerBody");
        this.playerBody.Connect("animation_finished", this, nameof(AttackAnimFinished));
        this.player = (KinematicBody2D)this.GetParent().GetParent();
        this.hitbox = (CollisionShape2D)playerBody.GetNode("SwordHitBox/CollisionShape2D");
        this.hitbox.Disabled = true;

        this.bulletSpawn = (Position2D)player.FindNode("BulletSpawn");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("attack_light"))
        {
            LightAttack();
        }

        if (Input.IsActionPressed("skill_1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!canShoot) return;
        RandomNumberGenerator rng = new RandomNumberGenerator();
        int shotCount = rng.RandiRange(14, 18);
        for (int i = 0; i < shotCount; i++)
        {
            rng.Randomize();
            float randomDegrees = rng.RandfRange(-10f, 10f);
            Bullet instance = (Bullet)bullet.Instance();
            GetTree().Root.AddChild(instance);
            instance.RotationDegrees = this.player.RotationDegrees + randomDegrees;
            instance.GlobalPosition = bulletSpawn.GlobalPosition;
        }

        Vector2 direction = new Vector2(Mathf.Cos(this.player.Rotation), Mathf.Sin(this.player.Rotation));
        ((IPushable)player).Push(-direction, selfShotgunForce);

        ResetShootAnimation();
        ResetShoot();
    }


    private async void ResetShoot()
    {
        canShoot = false;
        await ToSignal(GetTree().CreateTimer(shootCooldown), "timeout");
        canShoot = true;
    }

    private async void ResetShootAnimation()
    {
        canAttack = false;
        hitbox.Disabled = true;
        this.playerBody.Animation = "shoot";
        this.playerBody.Frame = 0;
        await ToSignal(GetTree().CreateTimer(0.15f), "timeout");
        this.playerBody.Animation = "attack_light";
        canAttack = true;
    }

    private void LightAttack()
    {
        if (canAttack)
        {
            ResetHitbox();
            playerBody.Play();
            ResetAttack();
        }
    }

    private async void ResetHitbox()
    {
        hitbox.Disabled = false;
        await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
        hitbox.Disabled = true;
    }

    private async void ResetAttack()
    {
        ResetAnimation();

        canAttack = false;
        await ToSignal(GetTree().CreateTimer(timeBetweenCombos), "timeout");
        canAttack = true;
    }

    private async void ResetAnimation()
    {
        await ToSignal(GetTree().CreateTimer(0.15f), "timeout");
        playerBody.Animation = "attack_light";
        playerBody.Frame = 0;
    }

    private void AttackAnimFinished(){
        playerBody.Stop();
    }

    public void OnSwordHitBoxBodyEnter(Godot.Object body)
    {
        if (body == player)
        {
            return;
        }

        if (body is IPushable)
        {
            ((IPushable)body).Push(player.Position.DirectionTo(((Node2D)body).Position), lightAttackPushingForce);
        }

        if (body is IDamageable)
        {
            ((IDamageable)body).ApplyDamage((Node)this, lightAttackDamage);
        }
    }
}