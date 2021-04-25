using Godot;

public class MainMenuAnimationPLayer : AnimationPlayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Wait();
    }

    private async void Wait(){
        Play("BackgroundAnimation");

        await ToSignal(GetTree().CreateTimer(4f), "timeout");

         AnimationPlayer secondAnimationPlayer = (AnimationPlayer)FindNode("SecondAnimationPlayer", true, true);
        secondAnimationPlayer.Play("BackgroundAnimation");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
