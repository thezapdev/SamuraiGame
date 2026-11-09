
using Godot;

public partial class JumpState : PlayerState
{
	[Export]
	public float JumpVelocity = -350f;

	public override void Enter()
	{
		Player.Velocity = new Vector2(
			Player.Velocity.X,
			JumpVelocity
		);

	}

	public override void PhysicsUpdate(double delta)
	{
		if (TryAttack())
			return;

		float dir = Player.GetInputDirection();

		Player.FaceDirection(dir);

		Player.Velocity = new Vector2(
			dir * Player.Stats.MoveSpeed,
			Player.Velocity.Y
		);

		Player.ApplyGravity(delta);

		if (Input.IsActionJustReleased("jump") && Player.Velocity.Y < 0)
		{
			Player.Velocity = new Vector2(
				Player.Velocity.X,
				Player.Velocity.Y * 0.5f
			);
		}

		if (Player.Velocity.Y >= 0)
			ChangeState("Fall");
	}
}
