using Godot;
using System;

public class GameManager : Node2D
{

    [Signal]
    public delegate void LevelCompleted(LevelResult result);

    [Signal]
    public delegate void GameOver(LevelResult result);

    [Export]
    public NodePath soundPlayerPath;

    public AudioStreamPlayer2D soundPlayer;

    private Label timerLabel;
    float altitude;

    LevelResult result;
    LevelResult prevLevelResult;

    int enemyCount = 0;

    private int wave;

    private int speedMultiplier = 1;

    private EnemySpawner spawner;

    private Player player;

    public GameManager()
    {
        result = new LevelResult();
    }

    public override void _Ready()
    {
        soundPlayer = GetNode<AudioStreamPlayer2D>(soundPlayerPath);
        timerLabel = (Label)FindNode("TimerLabel", true, false);
        spawner = new EnemySpawner(this.GetParent(), GetNode<CollisionShape2D>("Area2D/CollisionShape2D"), "res://scenes/enemy/");
        player = GetNode<Player>("Player");
        player.Connect("OnPlayerDeath", this, "OnPlayerDeath");
    }

    public async void StartLevel(int wave, int altitude, LevelResult prevLevelResult)
    {
        this.wave = wave;
        this.altitude = (float)altitude;
        this.prevLevelResult = prevLevelResult;

        GD.Print("Level " + wave + " starting");

        enemyCount = altitude / (30 + wave * 3);

        GD.Print("enemy count: " + enemyCount);

        string[] potentialEnemies;
        if (wave < 4)
        {
            potentialEnemies = new string[] { "Bat", "Rat" };
        }
        else if (wave < 7)
        {
            potentialEnemies = new string[] { "Bat", "Rat", "DemonWalker", "Hellhound" };
        }
        else
        {
            potentialEnemies = new string[] { "Bat", "Hellhound", "DemonWalker", "Rat" };
        }

        int c = enemyCount;
        await ToSignal(GetTree().CreateTimer(1f), "timeout");
        for (int i = 0; i < c; i++)
        {
            try
            {
                Enemy currentEnemey = spawner.SpawnRandomEnemy(potentialEnemies);
                currentEnemey.Connect("OnEnemyDeath", this, "OnEnemyDeath");
                GD.Print("Spawned enemy#" + i);

                await ToSignal(GetTree().CreateTimer(2f), "timeout");
            }
            catch (Exception e)
            {
                GD.Print(e.Message);

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

        PlayEnemyDeathSound(e.Name);

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

    private void PlayEnemyDeathSound(string name)
    {
        try
        {
            soundPlayer.Stream = (GD.Load<AudioStream>("res://sfx/enemies/" + name + ".mp3"));
            soundPlayer.Play();
        } catch(Exception e){
            GD.PrintErr(e.Message);
        }
    }

    private async void LeaveLevel()
    {
        await ToSignal(GetTree().CreateTimer(1.5f), "timeout");
        GD.Print("Level completed");
        var parameters = new object[1];

        result.waveNum = wave;
        result.altitude = (int)altitude;
        result.prev = prevLevelResult;

        parameters[0] = result;

        EmitSignal("LevelCompleted", parameters);
    }

    public void OnPlayerDeath(Node2D player)
    {
        GD.Print("Level completed");
        var parameters = new object[1];

        result.waveNum = wave;
        result.altitude = (int)altitude;
        result.prev = prevLevelResult;

        parameters[0] = result;

        EmitSignal("GameOver", parameters);
    }

    public override void _Process(float delta)
    {
        altitude += delta * 4.5f * speedMultiplier;

        timerLabel.Text = "Altitude\n-" + altitude.ToString("0.0") + "M";
    }

    public void setSpeed(int multiplier)
    {
        this.speedMultiplier = multiplier;
    }
}
