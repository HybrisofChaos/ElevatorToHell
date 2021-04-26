using Godot;
using System.Collections.Generic;

public class GroundStompFX : Node2D
{
    [Export]
    public float scaleSpeed = 3;

    [Export]
    public float maxScale = 10;

    [Export]
    public float pushForce = 20;

    [Signal]
    public delegate void EnemyEntered(Enemy e);

    private List<Enemy> enemiesInShockwave = new List<Enemy>();

    public override void _PhysicsProcess(float delta)
    {
        if(this.Scale == Vector2.One * maxScale){
            QueueFree();
            return;
        }

        Vector2 deltaScale = Vector2.One * scaleSpeed * delta;
        Vector2 newScale = this.Scale + deltaScale;
        if(newScale.x > maxScale){
            newScale.x = maxScale;
        }

        if(newScale.y > maxScale){
            newScale.y = maxScale;
        }

        this.Scale = newScale;
    }

    public override void _Process(float delta)
    {
        foreach(Enemy e in enemiesInShockwave){
            Vector2 direction = this.Position.DirectionTo(e.Position);
            e.Push(direction, pushForce, 500f);
        }
    }

    public void OnShockwaveBodyEntered(Godot.Object body){
        if(body is Enemy){
            Enemy e = (Enemy) body;
            enemiesInShockwave.Add(e);
        }
    }

    public void OnShockwaveBodyExited(Godot.Object body){
        if(body is Enemy){
            Enemy e = (Enemy) body;
            enemiesInShockwave.Remove(e);
        }
    }

}
