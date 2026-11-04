using Godot;
public partial class Enemy : CharacterBody2D
{
	[Export] private HealthComponent _healthComponent;
	[Export] private EnemyMovement _enemyMovement;
	[Export] public int ExpReward = 25;
	[Export] public CharacterBody2D TargetToChase;

	private bool _isDead = false;
	private AnimatedSprite2D _enemySprite;
	private Player _player;

	public override void _Ready()
	{
		_enemySprite = GetNode<AnimatedSprite2D>("EnemySprite2D");

		_player = GetTree().GetFirstNodeInGroup("Player") as Player;
		if (_player == null)
			GD.PrintErr("Nie znaleziono Player!");

		if (TargetToChase == null)
		{
			TargetToChase = GetTree().GetFirstNodeInGroup("Player") as CharacterBody2D;
		}

		if (_healthComponent != null)
		{
			_healthComponent.Died += OnDied;
			_healthComponent.HealthChanged += OnHealthChanged;
		}

		if (_enemyMovement == null)
		{
			GD.PrintErr($"Enemy [{Name}]: Brak przypisanego EnemyMovement w inspektorze!");
			return;
		}

		SetPhysicsProcess(false);
		CallDeferred(MethodName.WaitForPhysics);
	}

	public override void _ExitTree()
	{
		if (_healthComponent != null)
		{
			_healthComponent.Died -= OnDied;
			_healthComponent.HealthChanged -= OnHealthChanged;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		GD.Print("Enemy physics tick");
		_enemyMovement?.HandleMovement(delta);
	}

	public void TakeDamage(int damage)
	{
		if (_isDead) return;
		_healthComponent?.TakeDamage(damage);
	}

	private async void WaitForPhysics()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		SetPhysicsProcess(true);
	}

	private void OnHealthChanged(int currentHealth, int maxHealth)
	{
		if (_isDead) return;

		if (currentHealth > 0)
		{
			_enemySprite.Play("Hurt");
			GD.Print($"Enemy hp: {currentHealth}/{maxHealth}");
		}
	}

	private async void OnDied()
	{
		if (_isDead) return;
		_isDead = true;

		SetPhysicsProcess(false); // koniec ruchu
		_player?.KillEnemy(this);

		_enemySprite.Play("Dead");

		GetNode<CollisionShape2D>("CollisionShape2D")
			.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);

		await ToSignal(_enemySprite, AnimatedSprite2D.SignalName.AnimationFinished);

		QueueFree();
	}
}
