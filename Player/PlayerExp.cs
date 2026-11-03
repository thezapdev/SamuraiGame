
using Godot;

public partial class PlayerExp : Node
{
	private int _currentExp = 0;
	private int _maxExp = 100;

	private LevelSystem _levelSystem;

	[Signal]
	public delegate void ExpChangedEventHandler(int currentExp, int maxExp);

	public override void _Ready()
	{
		_levelSystem = GetNode<LevelSystem>("../LevelSystem");

		EmitSignal(SignalName.ExpChanged, _currentExp, _maxExp);
	}

	public void AddExp(int amount)
	{
		if (amount <= 0)
			return;

		GD.Print($"Dodano EXP: {amount}");

		_currentExp += amount;

		while (_currentExp >= _maxExp)
		{
			_currentExp -= _maxExp;
			_levelSystem.LevelUp();
			_maxExp = (int)(_maxExp * 1.2f);
		}

		EmitSignal(SignalName.ExpChanged, _currentExp, _maxExp);
	}

	public int GetCurrentExp() => _currentExp;
	public int GetMaxExp() => _maxExp;
}
