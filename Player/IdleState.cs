
using Godot;

public partial class IdleState : PlayerState
{
	public override void Enter()
	{
		Player.Velocity = new Vector2(0, Player.Velocity.Y);
		Player.Sprite.Play("Idle");
	}

	public override void PhysicsUpdate(double delta)
	{
		if (TryAttack())
			return;

		Player.ApplyGravity(delta);

		float direction = Player.GetInputDirection();

		if (Input.IsActionJustPressed("jump"))
			return;

		if (direction != 0)
		{
			Player.FaceDirection(direction);
			ChangeState("Run");
		}
	}
}
