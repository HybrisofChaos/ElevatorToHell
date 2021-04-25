using Godot;
using System;

public class SceneTransition : ColorRect
{
    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        SetSize(OS.WindowSize);
        this.SetSize(OS.WindowSize);

        Visible = false;
    }

    private void FaderAnimationStart(String anim_name)
    {
        Visible = true;
    }

    private void FaderAnimationEnd(String anim_name)
    {
        Visible = false;
    }

}
