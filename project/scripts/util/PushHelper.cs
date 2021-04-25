using Godot;

public class PushHelper {

    private Node2D subject;
    private Vector2 direction;
    private float force;
    private float speed;
    private OnPushFinished callback;

    private float pushAmount = 0;

    public delegate void OnPushFinished();

    public PushHelper(Node2D subject, Vector2 direction, float force, float speed, OnPushFinished callback){
        this.subject = subject;
        this.direction = direction;
        this.force = force;
        this.speed = speed;
        this.callback = callback;
    }

    public void Tick(float delta){
        if(pushAmount < force){
            float amountToPush = this.speed * delta;
            this.subject.Position += this.direction * amountToPush;
            this.pushAmount += amountToPush;
        }else{
            this.callback();
        }
    }
}