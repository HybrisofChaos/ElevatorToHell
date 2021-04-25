using Godot;
using System;

public class Rat : Enemy
{
    [Export]
    public int damagePerTick = 150;

    [Export]
    public float timePerDamageTick = 0.5f;

    private bool canAttack = true;

    private IDamageable attackTarget = null;
    private AnimatedSprite sprite;

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode<AnimatedSprite>("Sprite");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (attackTarget != null)
        {
            Attack();
        }

        if (this.isFollowingPath)
        {
            if (!sprite.IsPlaying())
            {
                sprite.Play();
            }
        }
        else
        {
            if (sprite.IsPlaying())
            {
                sprite.Stop();
            }
        }
    }

    public void OnAttackHitboxBodyEntered(Godot.Object body)
    {
        if (body is IDamageable && !(body is Enemy))
        {
            attackTarget = (IDamageable)body;
            Attack();
        }
    }

    public void OnAttackHitboxBodyExited(Godot.Object body)
    {
        if (body is IDamageable)
        {
            if (((IDamageable)body) == attackTarget)
            {
                attackTarget = null;
            }
        }
    }

    private void Attack()
    {
        if (canAttack)
        {
            shouldFollowPath = false;
            ResetAttack();
            attackTarget.ApplyDamage(this, damagePerTick);
        }
    }

    private async void ResetAttack()
    {
        canAttack = false;
        await ToSignal(GetTree().CreateTimer(timePerDamageTick), "timeout");
        canAttack = true;
        shouldFollowPath = true;
    }

}
