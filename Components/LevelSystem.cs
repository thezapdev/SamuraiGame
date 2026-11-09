using Godot;

public partial class LevelSystem : Node
{
	public int Level { get; private set; } = 1;
	public int CurrentExp { get; private set; } = 0;
	public int MaxExp { get; private set; } = 100;

	[Export] private StatsManager _statManager;

	[Signal]
	public delegate void ExpChangedEventHandler(int current, int max);

	[Signal]
	public delegate void LevelChangedEventHandler(int level);

	public override void _Ready()
	{
		EmitSignal(SignalName.ExpChanged, CurrentExp, MaxExp);
	}

	public void AddExp(int amount)
	{
		if (amount <= 0)
			return;

		GD.Print($"Dodano EXP: {amount}");

		CurrentExp += amount;

		while (CurrentExp >= MaxExp)
		{
			CurrentExp -= MaxExp;
			LevelUp();
		}

		EmitSignal(SignalName.ExpChanged, CurrentExp, MaxExp);
	}

	public void LevelUp()
	{
		Level++;
		MaxExp = (int)(MaxExp * 1.2f);

		_statManager.LevelUp();

		EmitSignal(SignalName.LevelChanged, Level);
	}

	public int GetCurrentExp() => CurrentExp;
	public int GetMaxExp() => MaxExp;
}
