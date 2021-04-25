using Godot;
using System;
using System.Collections.Generic;

public class LevelResult : Godot.Object
{
    //todo: idea
    //public Map<Enemy, Integer> enemyKills

    //void calculateXp(){
    //     return enemyKills.map(enemyKill => {
    //         return enemyKill.value * enemyKill.key.xp
    //     })
    // }

    public Dictionary<Enemy, int> enemies;

    public int waveNum = 0;

    public LevelResult(){
        enemies = new Dictionary<Enemy, int>();
    }
}
