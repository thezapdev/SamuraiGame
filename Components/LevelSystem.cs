using Godot;
using System;

public partial class LevelSystem : Node
{

	public int Level { get; private set; } = 1;
	public int CurrentExp { get; private set; } = 0;
	public int MaxExp { get; private set; } = 100;

	[Signal]
	public delegate void ExpChangedEventHandler(int current, int max);
	[Signal]
	public delegate void LevelChangedEventHandler(int level);

	public void AddExp(int amount)
	{
		CurrentExp += amount;

		while (CurrentExp >= MaxExp)
		{
			CurrentExp -= MaxExp;
			LevelUp();
		}
	}
	public void LevelUp()
	{
		Level++;
		MaxExp = (int)(MaxExp * 1.2);
		EmitSignal(SignalName.LevelChanged, Level);
	}
}
