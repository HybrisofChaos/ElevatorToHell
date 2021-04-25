using Godot;
using System;
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

    private async void Wait()
    {
        try
        {
            Play("BackgroundAnimation");

            await ToSignal(GetTree().CreateTimer(3f), "timeout");

            AnimationPlayer secondAnimationPlayer = (AnimationPlayer)FindNode("SecondAnimationPlayer", true, true);
            secondAnimationPlayer.Play("BackgroundAnimation");

            await ToSignal(GetTree().CreateTimer(3f), "timeout");

            AnimationPlayer thirdAnimationPlayer = (AnimationPlayer)FindNode("ThirdAnimationPlayer", true, false);
            thirdAnimationPlayer.Play("BackgroundAnimation");
        } catch(Exception e){
            GD.Print(e.Message);
        }
    }
}
