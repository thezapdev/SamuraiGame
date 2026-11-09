
using Godot;

public partial class AttackState : PlayerState
{
	[Export]
	public bool MoveWhileAttacking = true;

	private bool _animationFinished;

	public override void Enter()
	{
		GD.Print("PLAYER: ENTER ATTACK");

		_animationFinished = false;

		// Podłączenie zdarzenia animacji.
		Player.Sprite.AnimationFinished += OnAnimationFinished;

		// Aktywacja odpowiedniego hitboxa.
		Player.SetAttackHitbox(true);

		// Uruchomienie animacji.
		Player.Sprite.Play("Attack");

		// Kontrola, czy animacja istnieje i jest odtwarzana.
		if (!Player.Sprite.IsPlaying())
		{
			GD.PrintErr("AttackState: animacja Attack nie została uruchomiona!");
		}
	}

	public override void Exit()
	{
		GD.Print("PLAYER: EXIT ATTACK");

		Player.Sprite.AnimationFinished -= OnAnimationFinished;

		// Wyłączenie obu hitboxów.
		Player.SetAttackHitbox(false);
	}

	public override void PhysicsUpdate(double delta)
	{
		Player.ApplyGravity(delta);

		float vx = MoveWhileAttacking
			? Player.GetInputDirection() * Player.Stats.MoveSpeed
			: 0f;

		Player.Velocity = new Vector2(
			vx,
			Player.Velocity.Y
		);
	}

	private void OnAnimationFinished()
	{
		if (_animationFinished)
			return;

		if (Player.Sprite.Animation != "Attack")
			return;

		_animationFinished = true;

		GD.Print("PLAYER: ATTACK ANIMATION FINISHED");

		if (!Player.IsOnFloor())
		{
			ChangeState("Fall");
		}
		else
		{
			ChangeState(
				Player.GetInputDirection() != 0
					? "Run"
					: "Idle"
			);
		}
	}
}
