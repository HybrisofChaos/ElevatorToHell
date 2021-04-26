using Godot;
using System;

public class GameManager : Node2D
{
    [Signal]
    public delegate void LevelCompleted(LevelResult result);

    private Label timerLabel;
    float altitude;

    LevelResult result;
    LevelResult prevLevelResult;

    int enemyCount = 0;

    private int wave;

    private int speedMultiplier = 1;

    public GameManager()
    {
        result = new LevelResult();
    }

    public override void _Ready()
    {
        timerLabel = (Label)FindNode("TimerLabel", true, false);
    }

    public async void StartLevel(int wave, int altitude, LevelResult prevLevelResult)
    {
        this.wave = wave;
        this.altitude = (float)altitude;
        this.prevLevelResult = prevLevelResult;

        GD.Print("Level " + wave + " starting");

        enemyCount = altitude / 40;

        GD.Print("enemy count: " + enemyCount);

        string[] potentialEnemies;
        if (wave < 4)
        {
            potentialEnemies = new string[] { "Bat", "Rat" };
        }
        else if (wave < 7)
        {
            potentialEnemies = new string[] { "Bat", "Rat", "DemonWalker" };
        }
        else
        {
            potentialEnemies = new string[] { "Bat", "Hellhound", "DemonWalker" };
        }

        int c = enemyCount;

        Random rnd = new Random();
        for (int i = 0; i < c; i++)
        {
            int r = rnd.Next(potentialEnemies.Length);
            string chosenEnemy = potentialEnemies[r];

            PackedScene enemy = GD.Load<PackedScene>("res://scenes/enemy/" + chosenEnemy + ".tscn");
            Node enemyNode = enemy.Instance();

            enemyNode.Connect("OnEnemyDeath", this, "OnEnemyDeath");
            try
            {
                this.AddChild(enemyNode);

                GD.Print("Spawned enemy#" + i);

                await ToSignal(GetTree().CreateTimer(2f), "timeout");
            }
            catch (Exception e)
            {
                GD.Print(e.Message);

                this.RemoveChild(enemyNode);

                enemyCount--;

                if (enemyCount <= 0)
                {
                    LeaveLevel();
                }
            }
        }
    }

    public void OnEnemyDeath(Node2D enemy)
    {
        Enemy e = (Enemy)enemy;

        enemyCount--;

        GD.Print("enemy count: " + enemyCount);

        if (result.enemies.ContainsKey(e.monsterName))
        {
            result.enemies[e.monsterName].count += 1;
        }
        else
        {
            EnemyResult enemyResult = new EnemyResult()
            {
                xp = e.xp,
                count = 1
            };

            result.enemies.Add(e.monsterName, enemyResult);
        }

        if (enemyCount <= 0)
        {
            LeaveLevel();
        }
    }

    private void LeaveLevel()
    {
        GD.Print("Level completed");
        var parameters = new object[1];

        result.waveNum = wave;
        result.altitude = (int)altitude;
        result.prev = prevLevelResult;

        parameters[0] = result;

        EmitSignal("LevelCompleted", parameters);
    }

    public override void _Process(float delta)
    {
        altitude += delta * 7.5f * speedMultiplier;

        timerLabel.Text = "Altitude\n-" + altitude.ToString("0.0") + "M";
    }

    public void setSpeed(int multiplier)
    {
        this.speedMultiplier = multiplier;
    }
}
