using Godot;

public partial class DeadState : PlayerState
{
	public override void Enter()
	{
		Player.Velocity = Vector2.Zero;
	}

	public override void PhysicsUpdate(double delta)
	{
		Player.Velocity = new Vector2(0, Player.Velocity.Y);
		Player.ApplyGravity(delta);
	}
}
