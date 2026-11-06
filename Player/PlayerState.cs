
using Godot;

public partial class PlayerState : State
{
	protected Player Player;

	public override void _Ready()
	{
		Player = GetParent() as Player
			?? GetParent()?.GetParent() as Player;

		if (Player == null)
		{
			GD.PushError(
				$"{Name}: Nie znaleziono Player. " +
				$"Rodzic: {GetParent()?.Name}, " +
				$"rodzic rodzica: {GetParent()?.GetParent()?.Name}"
			);
		}
	}
}
