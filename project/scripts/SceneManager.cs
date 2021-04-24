using Godot;

public class SceneManager : Node2D
{
    public override void _Ready()
    {
        PackedScene scene = GD.Load<PackedScene>("res://levels/Game.tscn");
        var instance = scene.Instance();

        AddChild(instance);
    }
}
