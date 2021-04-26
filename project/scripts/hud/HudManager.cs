using Godot;
using System;

public class HudManager : Node2D
{

    [Export]
    public NodePath playerPath;

    [Export]
    public NodePath damageAnimationPlayerPath;

    [Export]
    public NodePath dodgeAnimationPlayerPath;

    private Player player;
    private AnimationPlayer damageAnimationPlayer;
    private AnimationPlayer dodgeAnimationPlayer;
    private TextureProgress healthBar;
    private TextureProgress staminaBar;

    private int prevHealth = 0;
    private float prevStamina = 0;

    public override void _Ready()
    {
        if (playerPath == null)
        {
            QueueFree();
        }
        else
        {
            player = GetNode<Player>(playerPath);
            damageAnimationPlayer = GetNode<AnimationPlayer>(damageAnimationPlayerPath);
            dodgeAnimationPlayer = GetNode<AnimationPlayer>(dodgeAnimationPlayerPath);

            healthBar = (TextureProgress)FindNode("HealthProgressBar", true, false);
            staminaBar = (TextureProgress)FindNode("StaminaProgressBar", true, false);

            base._Ready();
        }
    }

    public override void _Process(float delta)
    {
        if (player == null)
        {
            QueueFree();
        }
        else
        {
            healthBar.Value = player.GetHealth();
            staminaBar.Value = player.stamina;

            if (prevHealth > player.GetHealth())
            {
                damageAnimationPlayer.Play("TakeDamage");
            }

            if (prevStamina > player.stamina)
            {
                dodgeAnimationPlayer.Play("Dodge");
            }

            prevHealth = player.GetHealth();
            prevStamina = player.stamina;

            base._Process(delta);
        }
    }
}
