using Godot;
using System.Collections.Generic;

public class MovingDown : Node2D
{
    SceneManager parent;
    AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("LiftAnimationPlayer");

        animationPlayer.Connect("animation_finished", this, "LiftAnimationFinished");

        animationPlayer.Play("MoveLift");
    }

    public void OnLevelCompleted(LevelResult result, SceneManager parent)
    {
        this.parent = parent;

        VBoxContainer textContainer = (VBoxContainer)FindNode("TextContainer");
        VBoxContainer splitContainer = (VBoxContainer)FindNode("UISplitContainer");

        splitContainer.Set("custom_constants/separation", 260 - result.enemies.Count * 16);

        DynamicFont headerFont = new DynamicFont();
        headerFont.FontData = (DynamicFontData)ResourceLoader.Load("res://fonts/Comfortaa-Bold.ttf");
        headerFont.OutlineSize = 3;
        headerFont.OutlineColor = new Color("#000000");
        headerFont.Size = 36;

        Label waveCompletedLabel = new Label();
        waveCompletedLabel.Text = "Wave " + result.waveNum + " completed";
        waveCompletedLabel.AddFontOverride("font", headerFont);

        textContainer.AddChild(waveCompletedLabel);

        DynamicFont font = new DynamicFont();
        font.FontData = (DynamicFontData)ResourceLoader.Load("res://fonts/Comfortaa-Bold.ttf");
        font.OutlineSize = 2;
        font.OutlineColor = new Color("#000000");
        font.Size = 16;

        Label enemyHeaderLabel = new Label();
        enemyHeaderLabel.Text = "Enemies slain";
        enemyHeaderLabel.AddFontOverride("font", font);

        textContainer.AddChild(enemyHeaderLabel);

        int totalXP = 0;
        foreach (KeyValuePair<Enemy, int> entry in result.enemies)
        {
            Label label = new Label();
            label.Text = "    " + entry.Value + " x " + entry.Key.name;
            label.AddFontOverride("font", font);

            totalXP += entry.Key.xp * entry.Value;

            textContainer.AddChild(label);
        }

        Label enemyFooterLabel = new Label();
        enemyFooterLabel.Text = "Total xp gained";
        enemyFooterLabel.AddFontOverride("font", font);

        textContainer.AddChild(enemyFooterLabel);

        Label totalXpLabel = new Label();
        totalXpLabel.Text = "    " + totalXP;
        totalXpLabel.AddFontOverride("font", font);

        textContainer.AddChild(totalXpLabel);
    }

    public void LiftAnimationFinished(string name)
    {
        animationPlayer.Play("MoveBackground");
        MarginContainer uiContainer = (MarginContainer)FindNode("UIContainer");
        uiContainer.Visible = true;
    }

    public void Continue()
    {
        parent.OnLevelContinue();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
