
using Godot;

public partial class FallState : PlayerState
{
	public override void Enter()
	{
	}

	public override void PhysicsUpdate(double delta)
	{
		if (TryAttack())
			return;

		Player.ApplyGravity(delta);

		float direction = Player.GetInputDirection();

		Player.FaceDirection(direction);

		Player.Velocity = new Vector2(
			direction * Player.Stats.MoveSpeed,
			Player.Velocity.Y
		);

		if (Player.IsOnFloor())
		{
			ChangeState(direction != 0 ? "Run" : "Idle");
		}
	}
}
