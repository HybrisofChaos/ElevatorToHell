using Godot;

public class SceneManager : Node2D
{
	[Signal]
	public delegate void LevelCompleted(LevelResult result);

	[Signal]
	public delegate void StartNextLevel();

	Node currentScene;

	LevelResult previousLevelResult;

	AudioStreamPlayer2D musicPLayer;

	public override void _Ready()
	{
		previousLevelResult = new LevelResult();
		previousLevelResult.waveNum = 0;
		previousLevelResult.altitude = 100;

		musicPLayer = GetNode<AudioStreamPlayer2D>("MainMusicPlayer");
		musicPLayer.Play();

		PackedScene scene = GD.Load<PackedScene>("res://levels/MainMenu.tscn");
		currentScene = scene.Instance();

		this.Connect("LevelCompleted", this, "OnLevelCompleted");
		currentScene.Connect("StartNextLevel", this, "OnStartNextLevel");

		AddChild(currentScene);
	}

	//testing

	public void OnStartNextLevel()
	{
		PackedScene scene = GD.Load<PackedScene>("res://levels/Game.tscn");
		RemoveChild(currentScene);
		currentScene.Dispose();

		currentScene = scene.Instance();
		currentScene.Connect("ready", this, "OnLevelReady");
		currentScene.Connect("LevelCompleted", this, "OnLevelCompleted");

		musicPLayer.Stop();
		AddChild(currentScene);
	}

	public void OnLevelReady()
	{
		int waveNum = 1;
		int altitude = 100;
		if (previousLevelResult != null)
		{
			waveNum = previousLevelResult.waveNum + 1;
			altitude = previousLevelResult.altitude;
		}

		GameManager gameManager = (GameManager)FindNode("Game", true, false);
		gameManager.StartLevel(waveNum, altitude, previousLevelResult);
	}


	public void OnLevelContinue()
	{
		PackedScene scene = GD.Load<PackedScene>("res://levels/SceneTransition.tscn");
		RemoveChild(currentScene);
		currentScene.Dispose();

		currentScene = scene.Instance();
		currentScene.Connect("ready", this, "PlayFadeInAnimation");

		AddChild(currentScene);
	}

	public void PlayFadeInAnimation()
	{
		AnimationPlayer animationPlayer = (AnimationPlayer)FindNode("AnimationPlayer", true, false);
		Label delvingDeeperLabel = (Label)FindNode("DelvingDeeperLabel", true, false);
		delvingDeeperLabel.Visible = true;

		ColorRect colorRect = (ColorRect)FindNode("SceneTransitionRect", true, false);

		animationPlayer.Connect("animation_finished", colorRect, "FaderAnimationEnd");
		animationPlayer.Connect("animation_finished", this, "FadeInFinished");
		animationPlayer.Connect("animation_started", colorRect, "FaderAnimationStart");

		animationPlayer.Play("FadeOut");
	}

	public void FadeInFinished(string name)
	{
		OnStartNextLevel();
	}

	private void OnLevelCompleted(LevelResult result)
	{
		previousLevelResult = result;
		PackedScene scene = GD.Load<PackedScene>("res://levels/SceneTransition.tscn");
		RemoveChild(currentScene);
		currentScene.Dispose();

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

		animationPlayer.Play("FadeIn");
	}

	public void FadeOutFinished(string name)
	{
		PackedScene scene = GD.Load<PackedScene>("res://levels/MovingDown.tscn");
		RemoveChild(currentScene);
		currentScene.Dispose();

		currentScene = scene.Instance();
		currentScene.Connect("ready", this, "OnMovingDownMenuReady");

		AddChild(currentScene);
	}

	public void OnMovingDownMenuReady()
	{
		musicPLayer.Play();
		if (previousLevelResult == null)
		{
			previousLevelResult = new LevelResult();
			previousLevelResult.waveNum = 1;
		}

		MovingDown movingDown = (MovingDown)FindNode("MovingDown", true, false);
		movingDown.OnLevelCompleted(previousLevelResult, this);
	}
}
