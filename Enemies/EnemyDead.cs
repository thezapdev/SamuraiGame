using Godot;

public partial class EnemyDead : EnemyState
{
	public override async void Enter()
	{
		Enemy.Combat.CancelAttack();
		Enemy.Movement.Stop();

		Enemy.GetNode<CollisionShape2D>("CollisionShape2D")
			.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);

		Enemy.Player?.KillEnemy(Enemy);
		Enemy.Sprite.Play("Dead");

		await ToSignal(Enemy.Sprite, AnimatedSprite2D.SignalName.AnimationFinished);
		Enemy.QueueFree();
	}
}
