using Godot;
using System;

public class DemonWalker : Enemy
{
    [Export]
    public int damage = 50;

    private IDamageable attackTarget = null;
    private AnimatedSprite sprite;

    public override void _Ready()
    {
        base._Ready();

        sprite = GetNode<AnimatedSprite>("Sprite");
    }

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
                ResetAnimation();
                ResetAttack();
            }
        }
    }

    private async void ResetAnimation(){
        sprite.Animation = "attack";
        sprite.Frame = 1;
        await ToSignal(GetTree().CreateTimer(0.15f), "timeout");
        sprite.Frame = 0;
    }
}