using Godot;
using System;

public class EnemyHealthBarManager : Node2D
{
    [Export]
    public NodePath enemyPath;

    [Export]
    public NodePath barPath;

    [Export]
    public float distanceMultiplier = 1;

    private TextureProgress progressBar;

    private Enemy enemy;

    private Vector2 originalPosition;

    public override void _Ready()
    {
        if (enemyPath == null)
        {
            QueueFree();
        }
        else
        {
            originalPosition = Position;

            enemy = GetNode<Enemy>(enemyPath);

            progressBar = GetNode<TextureProgress>(barPath);

            progressBar.MaxValue = enemy.maxHealth;
        }
    }

    public override void _Process(float delta)
    {
        GlobalRotation = 0;

        Vector2 pos = enemy.GlobalPosition;
        pos.x -= 40 * distanceMultiplier;
        pos.y -= 35 * distanceMultiplier;

        GlobalPosition = pos;

        progressBar.Value = enemy.currentHealth;
    }

}
