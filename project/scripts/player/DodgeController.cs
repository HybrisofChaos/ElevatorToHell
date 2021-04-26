using Godot;
using System;

public class DodgeController : Node
{
    private bool canDodge = true;

    Player player;
    Timer timer;
    Timer cooldownTimer;
    GameManager gameManager;

    public override void _Ready()
    {
        gameManager = GetTree().Root.GetNode<GameManager>("Main/Game");
        player = GetTree().Root.GetNode<Player>("Main/Game/Player");
        timer = GetNode<Timer>("Timer");
        cooldownTimer = GetNode<Timer>("CooldownTimer");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("dodge"))
        {
            if (canDodge)
            {
                canDodge = false;
                Engine.TimeScale = 0.2f;

                gameManager.setSpeed(6);
                player.SetSpeed(4);

                timer.Start();
                cooldownTimer.Start();
            }

        }
    }

    public void OnDodgeEnd()
    {
        Engine.TimeScale = 1f;

        gameManager.setSpeed(1);
        player.SetSpeed(1);
    }

    public void OnCooldownCompleted()
    {
        canDodge = true;

    }

    public override void _ExitTree()
    {
        OnDodgeEnd();
    }
}
