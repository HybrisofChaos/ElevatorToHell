using Godot;
using System;

public class Enemy : KinematicBody2D, IDamageable
{

    private int health = 500;

    public void ApplyDamage(int damage){
        health -= damage;
        if(health < 0){
            QueueFree();
        }
    }
}
