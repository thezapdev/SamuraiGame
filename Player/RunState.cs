
using Godot;

public partial class RunState : PlayerState
{
    public override void Enter()
    {
        Player.Sprite.Play("Run");
    }

    public override void PhysicsUpdate(double delta)
    {
        if (TryAttack())
            return;

        Player.ApplyGravity(delta);

        float direction = Player.GetInputDirection();

        if (direction == 0)
        {
            ChangeState("Idle");
            return;
        }

        Player.FaceDirection(direction);

        Player.Velocity = new Vector2(
            direction * Player.Stats.MoveSpeed,
            Player.Velocity.Y
        );

        if (Input.IsActionJustPressed("jump"))
            ChangeState("Jump");
    }
}
