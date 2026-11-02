using Godot;

public partial class PlayerExp : Node
{
	private int _currentExp = 0;
	private int _maxExp = 100;
	private int _level = 1;

	[Signal]
	public delegate void ExpChangedEventHandler(int currentExp, int maxExp);

	[Signal]
	public delegate void LevelChangedEventHandler(int level);

	public override void _Ready()
	{
		EmitSignal(SignalName.ExpChanged, _currentExp, _maxExp);
		EmitSignal(SignalName.LevelChanged, _level);
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
		LevelUp();
	}

	EmitSignal(SignalName.ExpChanged, _currentExp, _maxExp);
}
	private void LevelUp()
	{
		_level++;
		_maxExp = (int)(_maxExp * 1.2f);

		EmitSignal(SignalName.LevelChanged, _level);

		GD.Print($"Awans na {_level}! Nowy max EXP: {_maxExp}");
	}

	public int GetCurrentExp() => _currentExp;
	public int GetMaxExp() => _maxExp;
	public int GetLevel() => _level;
}
