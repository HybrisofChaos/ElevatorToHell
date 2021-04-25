using Godot;
using System;

public class MainMenuScript : CanvasLayer
{
    [Signal]
    public delegate void StartNextLevel();

    
    public override void _Ready()
    {
        
    }

    public void OnButtonPressed(){
        EmitSignal("StartNextLevel");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
