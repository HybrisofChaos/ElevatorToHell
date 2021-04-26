using Godot;
using System;

public class BloodSpatter : CPUParticles2D
{
    public override void _Ready()
    {
        this.Emitting = true;
        RemoveMe();
    }

    private async void RemoveMe(){
        await ToSignal(GetTree().CreateTimer(this.Lifetime), "timeout");
        QueueFree();
    }
}
