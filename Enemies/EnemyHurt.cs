using Godot;

public partial class EnemyHurt : EnemyState
{
	public override void Enter()
	{
		Enemy.Combat.CancelAttack();
		Enemy.Sprite.Play("Hurt");
		Enemy.Sprite.AnimationFinished += OnFinished;
	}

	public override void Exit() => Enemy.Sprite.AnimationFinished -= OnFinished;

	private void OnFinished() => ChangeState("Chase");
}
