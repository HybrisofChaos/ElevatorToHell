using Godot;

public interface IPushable {
    void Push(Vector2 direction, float force, float speed = 750f);
}