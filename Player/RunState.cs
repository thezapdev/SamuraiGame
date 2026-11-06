
using Godot;

public partial class RunState : PlayerState
{
    public override void Enter()
    {
        Player.Sprite.Play("Run");
    }

    public override void PhysicsUpdate(double delta)
    {
        float direction = Player.GetInputDirection();

        Player.FaceDirection(direction);

        Player.Velocity = new Vector2(
            direction * Player.Stats.MoveSpeed,
            Player.Velocity.Y
        );

        Player.ApplyGravity(delta);

        if (!Player.IsOnFloor())
        {
            ChangeState("Fall");
            return;
        }

        if (Input.IsActionJustPressed("jump"))
        {
            ChangeState("Jump");
            return;
        }

        if (direction == 0)
        {
            ChangeState("Idle");
            return;
        }
    }

    public override void Exit()
    {
        // Bez dodatkowych zmian.
    }
}
