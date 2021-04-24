using Godot;

public class SceneManager : Node2D
{
    [Signal]
    public delegate void LevelCompleted(LevelResult result);

    Node currentScene;
    public override void _Ready()
    {
        PackedScene scene = GD.Load<PackedScene>("res://levels/Game.tscn");
        currentScene = scene.Instance();

        this.Connect("LevelCompleted", this, "OnLevelCompleted");

        AddChild(currentScene);
    }

    //testing
    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionPressed("space"))
        {
            //would emit this from enemy controller
            var parameters = new object[1];
            LevelResult test = new LevelResult();
            test.waveNum = 5;

            EmitSignal("LevelCompleted", parameters);
        }
    }

    LevelResult previousLevelResult;
    private void OnLevelCompleted(LevelResult result)
    {
        previousLevelResult = result;
        PackedScene scene = GD.Load<PackedScene>("res://levels/SceneTransition.tscn");
        RemoveChild(currentScene);

        currentScene = scene.Instance();
        currentScene.Connect("ready", this, "PlayFadeOutAnimation");

        AddChild(currentScene);
    }

    public void PlayFadeOutAnimation()
    {
        AnimationPlayer animationPlayer = (AnimationPlayer)FindNode("AnimationPlayer", true, false);
        ColorRect colorRect = (ColorRect)FindNode("SceneTransitionRect", true, false);

        animationPlayer.Connect("animation_finished", colorRect, "FaderAnimationEnd");
        animationPlayer.Connect("animation_finished", this, "FadeOutFinished");
        animationPlayer.Connect("animation_started", colorRect, "FaderAnimationStart");

        animationPlayer.Play("FadeOut");
    }

    public void FadeOutFinished(string name)
    {
        PackedScene scene = GD.Load<PackedScene>("res://levels/MovingDown.tscn");
        RemoveChild(currentScene);
        currentScene = scene.Instance();
        AddChild(currentScene);
    }
}
