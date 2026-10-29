using System.Threading.Tasks;
using Godot;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public int Health { get; set; } = 30;
	private bool _isDead = false;
	private AnimatedSprite2D _enemySprite;

	public override void _Ready()
	{
		_enemySprite = GetNode<AnimatedSprite2D>("EnemySprite2D");

	}

	public void TakeDamage(int damage)
	{
		Health -= damage;
		_enemySprite.Play("Hurt");
		GD.Print($"Enemy HP: {Health}");

		if (Health <= 0)
		{
			Die();
		}
	}
	
	private async Task Die()
	{
		_isDead = true;
		_enemySprite.Play("Dead");
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled",true);
		await ToSignal(_enemySprite, AnimatedSprite2D.SignalName.AnimationFinished);
		QueueFree();
	}
	
	
}
