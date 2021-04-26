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
		this.EmitSignal(nameof(StartNextLevel));
	}
}
