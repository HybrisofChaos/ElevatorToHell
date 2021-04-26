using Godot;
using System.Collections.Generic;

public class GameOver : Node2D
{
    SceneManager parent;

    public override void _Ready()
    {

    }

    public void OnLevelCompleted(LevelResult result, SceneManager parent)
    {
        this.parent = parent;

        VBoxContainer textContainer = (VBoxContainer)FindNode("TextContainer");
        VBoxContainer splitContainer = (VBoxContainer)FindNode("UISplitContainer");

        splitContainer.Set("custom_constants/separation", 200 - result.enemies.Count * 18);

        DynamicFont headerFont = new DynamicFont();
        headerFont.FontData = (DynamicFontData)ResourceLoader.Load("res://fonts/Comfortaa-Bold.ttf");
        headerFont.OutlineSize = 3;
        headerFont.OutlineColor = new Color("#000000");
        headerFont.Size = 36;

        Label waveCompletedLabel = new Label();
        waveCompletedLabel.Text = "Game over";
        waveCompletedLabel.AddFontOverride("font", headerFont);

        textContainer.AddChild(waveCompletedLabel);

        DynamicFont waveClearedFont = new DynamicFont();
        waveClearedFont.FontData = (DynamicFontData)ResourceLoader.Load("res://fonts/Comfortaa-Bold.ttf");
        waveClearedFont.OutlineSize = 3;
        waveClearedFont.OutlineColor = new Color("#000000");
        waveClearedFont.Size = 24;

        Label waveClearedLabel = new Label();
        waveClearedLabel.Text = "Reached wave " + result.waveNum + " at -" + result.altitude + "M";
        waveClearedLabel.AddFontOverride("font", waveClearedFont);

        textContainer.AddChild(waveClearedLabel);

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
        foreach (KeyValuePair<string, EnemyResult> entry in result.getEnemies())
        {
            Label label = new Label();
            label.Text = "    " + entry.Value.count + " x " + entry.Key;
            label.AddFontOverride("font", font);

            totalXP += entry.Value.xp * entry.Value.count;

            textContainer.AddChild(label);
        }

        Label totalTotalXpLabel = new Label();
        totalTotalXpLabel.Text = "Total xp";
        totalTotalXpLabel.AddFontOverride("font", font);

        textContainer.AddChild(totalTotalXpLabel);

        Label totalTotalTotalXpLabel = new Label();
        totalTotalTotalXpLabel.Text = "    " + result.getTotalXp();
        totalTotalTotalXpLabel.AddFontOverride("font", font);

        textContainer.AddChild(totalTotalTotalXpLabel);
    }

    public void OnMainMenu(){
        parent.Restart();
    }
}
