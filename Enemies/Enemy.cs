using Godot;

public partial class Enemy : CharacterBody2D
{
	[Export] private HealthComponent _healthComponent;
	[Export] public EnemyMovement Movement;
	[Export] public EnemyCombat Combat;
	[Export] public StateMachine StateMachine;

	[ExportGroup("AI")]
	[Export] public float DetectionRange = 400f;

	[ExportGroup("Rewards")]
	[Export] public int ExpReward = 25;

	public AnimatedSprite2D Sprite { get; private set; }
	public Player Player { get; private set; }

	public override void _Ready()
	{
		Sprite = GetNode<AnimatedSprite2D>("EnemySprite2D");
		Movement ??= GetNode<EnemyMovement>("EnemyMovement");
		Combat ??= GetNode<EnemyCombat>("EnemyCombat");
		StateMachine ??= GetNode<StateMachine>("StateMachine");

		Player = GetTree().GetFirstNodeInGroup("Player") as Player;
		if (Player == null) GD.PrintErr("Enemy: Nie znaleziono Player!");

		if (_healthComponent != null)
		{
			_healthComponent.Died += OnDied;
			_healthComponent.HealthChanged += OnHealthChanged;
		}
		if (Player != null)
			AddCollisionExceptionWith(Player);
	}
	public override void _PhysicsProcess(double delta)
	{
		if (IsDead || !GodotObject.IsInstanceValid(Player))
			return;

		Movement.MoveTowards(Player.GlobalPosition.X);
		FaceTarget();
		Movement.Move(delta); // jedyny MoveAndSlide
		GD.Print($"vel.X={Velocity.X:F0} real.X={GetRealVelocity().X:F0} wall={IsOnWall()}");
	}
	public override void _ExitTree()
	{
		if (_healthComponent != null)
		{
			_healthComponent.Died -= OnDied;
			_healthComponent.HealthChanged -= OnHealthChanged;
		}
	}

	public bool IsDead => StateMachine.Current is EnemyDead;

	public bool IsPlayerInDetectionRange()
		=> GodotObject.IsInstanceValid(Player)
			&& GlobalPosition.DistanceTo(Player.GlobalPosition) <= DetectionRange;

	public void FaceTarget()
	{
		if (!GodotObject.IsInstanceValid(Player)) return;
		Sprite.FlipH = Player.GlobalPosition.X < GlobalPosition.X;
	}

	public void TakeDamage(int damage)
	{
		if (IsDead) return;
		_healthComponent?.TakeDamage(damage);
	}

	private void OnHealthChanged(int current, int max)
	{
		if (IsDead || current <= 0) return;
		StateMachine.ForceTransition("Hurt");
	}

	private void OnDied() => StateMachine.ForceTransition("Dead");

}
