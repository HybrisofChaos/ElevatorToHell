using Godot;
using System;

public class Bat : Enemy
{

    [Export]
    public int damage = 50;

    private IDamageable attackTarget = null;

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (attackTarget != null)
        {
            Attack();
        }
    }

    public void OnHitboxBodyEntered(Godot.Object body)
    {
        if (!(body is Enemy))
        {
            if (body is IDamageable)
            {
                attackTarget = (IDamageable)body;
            }
        }
    }

    public void OnHitboxBodyExited(Godot.Object body)
    {
        if (body == attackTarget)
        {
            attackTarget = null;
        }
    }

    private void Attack()
    {
        if (attackTarget != null)
        {
            if (canAttack)
            {
                attackTarget.ApplyDamage(this, damage);
                ResetAttack();
            }
        }
    }


    protected override void BeforeContinuePath(Vector2 target)
    {
        // Just for override purposes
    }

    public override void Push(Vector2 direction, float force, float speed = 750f)
    {
        // Can't push bats man.
    }
}