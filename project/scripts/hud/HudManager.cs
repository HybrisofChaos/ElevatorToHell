using Godot;
using System;

public class HudManager : MarginContainer
{

    [Export]
    public NodePath playerPath;

    private Player player;
    private TextureProgress healthBar;
    private TextureProgress staminaBar;

    public override void _Ready()
    {
        player = GetNode<Player>(playerPath);

        healthBar = (TextureProgress)FindNode("HealthProgressBar", true, false);
        staminaBar = (TextureProgress)FindNode("StaminaProgressBar", true, false);

        base._Ready();
    }

    public override void _Process(float delta)
    {
        healthBar.Value = player.GetHealth();
        staminaBar.Value = player.stamina;

        base._Process(delta);
    }

}
