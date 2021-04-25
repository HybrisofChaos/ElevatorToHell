using Godot;
using System;
using System.Collections.Generic;

public class LevelResult : Godot.Object
{
    public Dictionary<string, EnemyResult> enemies;

    public int waveNum = 0;
    public int altitude = 0;

    public LevelResult prev;

    public LevelResult(){
        enemies = new Dictionary<string, EnemyResult>();
    }

    public int getTotalXp(){
        int result = 0;

        foreach (KeyValuePair<string, EnemyResult> entry in enemies)
		{
			result += entry.Value.xp * entry.Value.count;
		}

        if(prev != null){
            result += prev.getTotalXp();
        }

        return result;
    }
}
