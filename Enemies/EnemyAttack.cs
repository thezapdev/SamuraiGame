using Godot;

public partial class EnemyAttack : EnemyState
{
	public override void Enter()
	{
		Enemy.Combat.AttackFinished += OnAttackFinished;
		Enemy.Combat.StartAttack();
	}

	public override void Exit() => Enemy.Combat.AttackFinished -= OnAttackFinished;

	private void OnAttackFinished() => ChangeState("Chase");
}
