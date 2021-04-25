using Godot;
using System;
using System.Diagnostics;

public class WeaponController : Node
{
    [Export]
    public float minTimeBetweenLightAttacks = 0.3f;
    [Export]
    public float maxTimeBetweenLightAttacks = 0.6f;
    [Export]
    public float timeBetweenCombos = 0.4f;
    [Export]
    public float comboCount = 2;

    [Export]
    public int lightAttackDamage = 100;

    [Export]
    public float lightAttackPushingForce = 100f;

    private bool canAttack = true;
    private bool canContinueCombo = true;

    private int currentComboPosition = 0;

    private float attackTimer = 0f;

    private AnimatedSprite playerBody;
    private KinematicBody2D player;

    // When implementing heavy attack we need two hitboxes.
    private CollisionShape2D hitbox;

    public override void _Ready()
    {
        this.playerBody = (AnimatedSprite)GetNode("../PlayerBody");
        this.player = (KinematicBody2D) this.GetParent().GetParent();
        this.hitbox = (CollisionShape2D)playerBody.GetNode("SwordHitBox/CollisionShape2D");
        this.hitbox.Disabled = true;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("attack_light"))
        {
            LightAttack();
        }

        DoComboCounter(delta);
    }

    private void DoComboCounter(float delta)
    {
        if (canAttack && currentComboPosition > 0)
        {
            attackTimer += delta;

            if (!canContinueCombo)
            {
                if (attackTimer >= minTimeBetweenLightAttacks)
                {
                    canContinueCombo = true;
                }
            }
            else
            {
                if (attackTimer > maxTimeBetweenLightAttacks)
                {
                    attackTimer = 0;
                    currentComboPosition = 0;
                    ResetAttack();
                }
            }
        }
        else
        {
            attackTimer = 0;
        }
    }

    private void LightAttack()
    {
        if (canAttack)
        {
            if (canContinueCombo)
            {
                ResetHitbox();
                currentComboPosition++;
                canContinueCombo = false;

                attackTimer = 0;

                playerBody.Frame = currentComboPosition;

                if (currentComboPosition == comboCount)
                {
                    currentComboPosition = 0;

                    ResetAttack();
                }
            }
        }
    }

    private async void ResetHitbox(){
        hitbox.Disabled = false;
        await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
        hitbox.Disabled = true;
    }

    private async void ResetAttack()
    {
        ResetAnimation();

        canContinueCombo = false;
        canAttack = false;
        await ToSignal(GetTree().CreateTimer(timeBetweenCombos), "timeout");
        canAttack = true;
        canContinueCombo = true;
    }

    private async void ResetAnimation()
    {
        await ToSignal(GetTree().CreateTimer(0.15f), "timeout");
        playerBody.Frame = 0;
    }

    public void OnSwordHitBoxBodyEnter(Godot.Object body)
    {
        if (body is IDamageable)
        {
            if (body == player)
            {
                return;
            }
            ((IDamageable)body).ApplyDamage((Node)this, lightAttackDamage);
        }

        if(body is IPushable){
            ((IPushable)body).Push(player.Position.DirectionTo(((Node2D) body).Position), lightAttackPushingForce);
        }
    }
}