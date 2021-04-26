using Godot;

public class PushHelper
{

    private Node2D subject;
    private Vector2 direction;
    private float force;
    private float speed;
    private OnPushFinished callback;

    private float pushAmount = 0;

    private bool finished = false;

    public delegate void OnPushFinished();

    public PushHelper(Node2D subject, Vector2 direction, float force, float speed, OnPushFinished callback)
    {
        this.subject = subject;
        this.direction = direction;
        this.force = force;
        this.speed = speed;
        this.callback = callback;
    }

    public void Tick(float delta)
    {
        if (pushAmount < force)
        {
            float amountToPush = this.speed * delta;
            if (!((KinematicBody2D) subject).TestMove(subject.Transform, this.direction * amountToPush))
            {
                this.subject.Position += this.direction * amountToPush;
            }
            this.pushAmount += amountToPush;
        }
        else
        {
            this.callback();
            this.finished = true;
        }
    }

    public void Add(Vector2 direction, float force, float speed = -1)
    {
        this.direction = (direction + this.direction).Normalized();
        this.force += force;
        this.speed += speed == -1 ? 0 : speed;
    }

    public bool isFinished()
    {
        return this.finished;
    }
}