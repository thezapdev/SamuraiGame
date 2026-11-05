using Godot;

public partial class ExpBar : ProgressBar
{
	[Export] private LevelSystem _levelSystem;

	public override void _Ready()
	{

		if (_levelSystem == null)
		{
			GD.PrintErr("Nie znaleziono LevelSystem");
			return;
		}

		_levelSystem.ExpChanged += UpdateExpBar;

		UpdateExpBar(
			_levelSystem.GetCurrentExp(),
			_levelSystem.GetMaxExp()
		);
	}

	private void UpdateExpBar(int currentExp, int maxExp)
	{
		MaxValue = maxExp;
		Value = currentExp;
	}
}
