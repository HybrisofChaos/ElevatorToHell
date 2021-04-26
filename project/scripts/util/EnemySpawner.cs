using Godot;
using System;

public class EnemySpawner
{

    private Node parent;
    private CollisionShape2D spawnPlatform;
    private string enemyScenePath;

    public EnemySpawner(Node parent, CollisionShape2D spawnPlatform, string enemyScenePath)
    {
        this.parent = parent;
        this.spawnPlatform = spawnPlatform;
        this.enemyScenePath = enemyScenePath + "{0}.tscn";
    }

    public Enemy SpawnEnemy(string enemySceneName)
    {
        string fullPath = string.Format(enemyScenePath, enemySceneName);
        PackedScene enemyScene = GD.Load<PackedScene>(fullPath);

        Vector2 spawnPoint = GetRandomSpawnPoint();

        Enemy enemy = enemyScene.Instance<Enemy>();
        this.parent.AddChild(enemy);
        enemy.GlobalPosition = spawnPlatform.GlobalPosition + spawnPoint;
        GD.Print(enemy.GlobalPosition);

        return enemy;
    }

    public Enemy SpawnRandomEnemy(string[] enemySceneNames){
        Random random = new Random();
        int index = random.Next(enemySceneNames.Length);

        return SpawnEnemy(enemySceneNames[index]);
    }

    private Vector2 GetRandomSpawnPoint()
    {
        RectangleShape2D spawnShape = (RectangleShape2D)this.spawnPlatform.Shape;
        int spawnPlatformExtentX = (int)spawnShape.Extents.x;
        int spawnPlatformExtentY = (int)spawnShape.Extents.y;
        Random random = new Random();
        int x = random.Next(-spawnPlatformExtentX, spawnPlatformExtentX);
        int y = random.Next(-spawnPlatformExtentY, spawnPlatformExtentY);

        return new Vector2(x, y);
    }
}