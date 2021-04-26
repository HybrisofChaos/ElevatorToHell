using Godot;
using System;
using System.Collections.Generic;

public class LevelResult : Godot.Object
{
    public Dictionary<string, EnemyResult> enemies;

    public int waveNum = 0;
    public int altitude = 0;

    public LevelResult prev;

    public LevelResult()
    {
        enemies = new Dictionary<string, EnemyResult>();
    }

    public Dictionary<string, EnemyResult> getEnemies()
    {
        if (prev != null)
        {
            foreach (KeyValuePair<string, EnemyResult> entry in prev.getEnemies())
            {
                if (enemies.ContainsKey(entry.Key))
                {
                    EnemyResult newResult = enemies[entry.Key];
                    newResult.count = enemies[entry.Key].count + 1;
                    enemies[entry.Key] = newResult;
                }
                else
                {
                    enemies.Add(entry.Key, entry.Value);
                }
            }
        }

        return enemies;
    }

    public int getTotalXp()
    {
        int result = 0;

        foreach (KeyValuePair<string, EnemyResult> entry in enemies)
        {
            result += entry.Value.xp * entry.Value.count;
        }

        if (prev != null)
        {
            result += prev.getTotalXp();
        }

        return result;
    }
}
