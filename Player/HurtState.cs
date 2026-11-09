using Godot;

public partial class HurtState : PlayerState
{
	[Export] public float Duration = 0.3f;

	private float _timer;

	public override void Enter()
	{
		_timer = Duration;

	}

	public override void PhysicsUpdate(double delta)
	{
		Player.Velocity = new Vector2(0, Player.Velocity.Y);
		Player.ApplyGravity(delta);

		_timer -= (float)delta;
		if (_timer > 0) return;

		ChangeState(Player.IsOnFloor() ? "Idle" : "Fall");
	}
}
