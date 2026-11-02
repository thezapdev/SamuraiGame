using Godot;

public partial class ExpBar : ProgressBar
{
	private PlayerExp _playerExp;

	public override void _Ready()
	{
		_playerExp = GetNodeOrNull<PlayerExp>("/root/Node2D/Player/PlayerExp");

		if (_playerExp == null)
		{
			GD.PrintErr("Nie znaleziono PlayerExp!");
			return;
		}

		_playerExp.ExpChanged += UpdateExpBar;

		UpdateExpBar(
			_playerExp.GetCurrentExp(),
			_playerExp.GetMaxExp()
		);
	}

	private void UpdateExpBar(int currentExp, int maxExp)
	{
		MaxValue = maxExp;
		Value = currentExp;
	}
}
