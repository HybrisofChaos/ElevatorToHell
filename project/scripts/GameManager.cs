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

        enemyCount = altitude / 20;

        GD.Print("enemy count: " + enemyCount);

        string[] potentialEnemies;
        if (wave < 4)
        {
            potentialEnemies = new string[] { "Bat", "Rat" };
        }
        else if (wave < 7)
        {
            potentialEnemies = new string[] { "Bat", "Rat", "Zombie" };
        }
        else
        {
            potentialEnemies = new string[] { "Bat", "Hound", "Zombie" };
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

            this.AddChild(enemyNode);

            GD.Print("Spawned enemy#" + i);

            await ToSignal(GetTree().CreateTimer(2f), "timeout");
        }
    }


    public void OnEnemyDeath(Node2D enemy)
    {
        Enemy e = (Enemy)enemy;

        enemyCount--;

        GD.Print("enemy count: " + enemyCount);

        if (result.enemies.ContainsKey(e.name))
        {
            result.enemies[e.name].count += 1;
        }
        else
        {
            EnemyResult enemyResult = new EnemyResult(){
                xp = e.xp,
                count = 1
            };

            result.enemies.Add(e.name, enemyResult);
        }

        if (enemyCount <= 0)
        {
            GD.Print("Level completed");
            var parameters = new object[1];

            result.waveNum = wave;
            result.altitude = (int)altitude;
            result.prev = prevLevelResult;

            parameters[0] = result;

            EmitSignal("LevelCompleted", parameters);
        }
    }

    public override void _Process(float delta)
    {
        altitude += delta * 2.5f;

        timerLabel.Text = "Altitude\n-" + altitude.ToString("0.0") + "M";
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionPressed("space"))
        {
            var parameters = new object[1];
            LevelResult result = new LevelResult();

            result.waveNum = wave;
            result.altitude = (int)altitude;

            parameters[0] = result;

            EmitSignal("LevelCompleted", parameters);
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
