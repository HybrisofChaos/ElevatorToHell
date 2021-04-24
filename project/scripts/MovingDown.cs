using Godot;

public class MovingDown : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    SceneTransition sceneTransition;
    AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        MenuButton continueButton = GetNode<MenuButton>("ContinueButton");
        animationPlayer = GetNode<AnimationPlayer>("LiftAnimationPlayer");

        animationPlayer.Connect("animation_finished", this, "LiftAnimationFinished");

        animationPlayer.Play("MoveLift");

        //continueButton.Connect("pressed", continueButton, "Continue");
    }

     public void LiftAnimationFinished(string name){
         animationPlayer.Play("MoveBackground");
     }

    public void Continue(){
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
