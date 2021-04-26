using Godot;
using System;
public class MainMenuAnimationPLayer : AnimationPlayer
{
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
