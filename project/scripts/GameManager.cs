using Godot;
using System;

public class GameManager : Node2D
{
    [Signal]
    public delegate void LevelCompleted(LevelResult result);

    private int wave;

    public override void _Ready()
    {

    }

    public void StartLevel(int wave, int time){
        this.wave = wave;
        GD.Print("Level " + wave + " starting");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionPressed("space"))
        {
            //would emit this from enemy controller
            var parameters = new object[1];
            LevelResult result = new LevelResult();
            result.waveNum = wave;
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
