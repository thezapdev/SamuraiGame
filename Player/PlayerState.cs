
using Godot;

public partial class PlayerState : State
{
	protected Player Player;

	public override void _Ready()
	{
		Player = GetParent().GetParent() as Player;

		if (Player == null)
			GD.PushError($"{Name}: nie znaleziono Playera");
	}

	protected bool TryAttack()
	{
		if (Input.IsActionJustPressed("Attack"))
		{
			ChangeState("Attack");
			return true;
		}

		return false;
	}
}
