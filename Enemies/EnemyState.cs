using Godot;

public partial class EnemyState : State
{
	protected Enemy Enemy;

	public override void _Ready()
	{
		Enemy = GetParent().GetParent() as Enemy;
		if (Enemy == null)
			GD.PushError($"{Name}: nie znaleziono Enemy");
	}

	protected bool TryAttack()
	{
		if (Enemy.Combat.CanAttack())
		{
			ChangeState("Attack");
			return true;
		}
		return false;
	}

	protected bool TryChase()
	{
		if (Enemy.IsPlayerInDetectionRange())
		{
			ChangeState("Chase");
			return true;
		}
		return false;
	}
}
