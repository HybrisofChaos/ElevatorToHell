using Godot;

public class MovingDown : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    SceneTransition sceneTransition;

    public override void _Ready()
    {
        sceneTransition = GetNode<SceneTransition>("SceneTransitionRect");
        MenuButton continueButton = GetNode<MenuButton>("ContinueButton");
        continueButton.Connect("pressed", continueButton, "Continue");
    }

    public void Continue(){
        sceneTransition._Transition_to("res://levels/Game.tscn");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
