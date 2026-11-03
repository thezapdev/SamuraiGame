using System.Threading.Tasks;
using Godot;

public partial class Enemy : CharacterBody2D
{
	[Export] private HealthComponent _healthComponent;
	[Export] public int ExpReward = 25;

	private bool _isDead = false;
	private AnimatedSprite2D _enemySprite;
	private Player _player;

	public override void _Ready()
	{
		_enemySprite = GetNode<AnimatedSprite2D>("EnemySprite2D");

		_player = GetTree().GetFirstNodeInGroup("Player") as Player;

		if (_player == null)
			GD.PrintErr("Nie znaleziono Player!");

		if (_healthComponent != null)
		{
			_healthComponent.Died += OnDied;
			_healthComponent.HealthChanged += OnHealthChanged;
		}
	}

	public void TakeDamage(int damage)
	{
		if (_isDead) return;

		_healthComponent?.TakeDamage(damage);
	}

	private void OnHealthChanged(int currentHealth, int maxHealth)
	{
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

		_player?.KillEnemy(this);

		_enemySprite.Play("Dead");

		GetNode<CollisionShape2D>("CollisionShape2D")
			.SetDeferred("disabled", true);

		await ToSignal(
			_enemySprite,
			AnimatedSprite2D.SignalName.AnimationFinished
		);

		QueueFree();
	}
}
