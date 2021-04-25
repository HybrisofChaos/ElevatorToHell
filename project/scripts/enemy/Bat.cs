using Godot;
using System;

public class Bat : Enemy
{

    [Export]
    public int damage = 50;

    [Export]
    public float damageInterval = 0.2f;

    private bool canAttack = true;

    private IDamageable attackTarget = null;

    public override void _Process(float delta){
        base._Process(delta);

        if(attackTarget != null){
            Attack();
        }
    }

    public void OnHitboxBodyEntered (Godot.Object body){
        if(!(body is Enemy)){
            if(body is IDamageable){
                attackTarget = (IDamageable) body;
            }
        }
    }

    public void OnHitboxBodyExited(Godot.Object body){
        if(body == attackTarget){
            attackTarget = null;
        }
    }

    private void Attack(){
        if(attackTarget != null){
            if(canAttack){
                attackTarget.ApplyDamage(this, damage);
                ResetAttack();
            }
        }
    }

    private async void ResetAttack(){
        canAttack = false;
        await ToSignal(GetTree().CreateTimer(damageInterval), "timeout");
        canAttack = true;
    }


    protected override void BeforeContinuePath(Vector2 target){
        // Just for override purposes
    }

    public override void Push(Vector2 direction, float force){
        // Can't push bats man.
    }
}