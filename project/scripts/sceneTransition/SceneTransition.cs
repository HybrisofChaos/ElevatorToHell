using Godot;
using System;

public class SceneTransition : ColorRect
{
    [Export]
	public String changeTo;

    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.PlayBackwards("Fade");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    // public override void _Process(float delta)
    // {
    // }

	public void _Transition_to(String nextScene){
        changeTo = nextScene;
		animationPlayer.Play("Fade");
		animationPlayer.Connect("animationFinished", animationPlayer, "AnimFinished");
	}

    public void AnimFinished(){
		GetTree().ChangeScene(changeTo);
    }
}
