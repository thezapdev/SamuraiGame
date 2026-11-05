
using Godot;

public partial class Enemy : CharacterBody2D
{
	[Export] private HealthComponent _healthComponent;

	[ExportGroup("Movement")]
	[Export] public float Speed = 180.0f;
	[Export] public float Gravity = 980.0f;

	[ExportGroup("Rewards")]
	[Export] public int ExpReward = 25;

	[Export] public CharacterBody2D TargetToChase;

	private bool _isDead = false;
	private AnimatedSprite2D _enemySprite;
	private Player _player;
	[Export] private EnemyMovement _enemyMovement;

	public override void _Ready()
	{
		_enemySprite = GetNode<AnimatedSprite2D>("EnemySprite2D");
		_enemyMovement = GetNode<EnemyMovement>("EnemyMovement");

		// Pobieranie gracza
		_player = GetTree().GetFirstNodeInGroup("Player") as Player;

		if (_player == null)
			GD.PrintErr("Enemy: Nie znaleziono Player!");
		_enemyMovement.SetTarget(_player);
		if (TargetToChase == null)
		{
			TargetToChase = _player as CharacterBody2D;
		}

		// Podłączanie HealthComponent
		if (_healthComponent != null)
		{
			_healthComponent.Died += OnDied;
			_healthComponent.HealthChanged += OnHealthChanged;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_isDead)
			return;

		_enemyMovement.HandleMovement(delta);
	}



	public override void _ExitTree()
	{
		if (_healthComponent != null)
		{
			_healthComponent.Died -= OnDied;
			_healthComponent.HealthChanged -= OnHealthChanged;
		}
	}

	public void TakeDamage(int damage)
	{
		if (_isDead)
			return;

		_healthComponent?.TakeDamage(damage);
	}

	private void OnHealthChanged(int currentHealth, int maxHealth)
	{
		if (_isDead)
			return;

		if (currentHealth > 0)
		{
			_enemySprite.Play("Hurt");
			GD.Print($"Enemy hp: {currentHealth}/{maxHealth}");
		}
	}

	private async void OnDied()
	{
		if (_isDead)
			return;

		_isDead = true;

		SetPhysicsProcess(false);

		_player?.KillEnemy(this);

		_enemySprite.Play("Dead");

		GetNode<CollisionShape2D>("CollisionShape2D")
			.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);

		await ToSignal(
			_enemySprite,
			AnimatedSprite2D.SignalName.AnimationFinished
		);

		QueueFree();
	}
}
