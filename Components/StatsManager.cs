using Godot;

public partial class StatsManager : Node
{
	[Export] public int CurrentLevel { get; private set; } = 1;
	public AttackSystem Attack { get; private set; }
	public CriticalSystem Critical { get; private set; }
	public DefenseSystem Defense { get; private set; }

	public override void _Ready()
	{
		Attack = new AttackSystem(baseDamage: 10, damagePerLevel: 3);
		Critical = new CriticalSystem(0.05f, 1.5f, 0.01f);
		Defense = new DefenseSystem(baseMaxHealth: 100f, healthPerLevel: 20f);
	}

	public void LevelUp()
	{
		CurrentLevel++;

		Attack.UpdateStats(CurrentLevel);
		Critical.UpdateStats(CurrentLevel);
		Defense.UpdateStats(CurrentLevel);

		GD.Print($"Level Up {CurrentLevel}! DMG: {Attack.CurrentDamage}, Crit: {Critical.CurrentCritChance * 100}%");
	}
	public int GetCurrentLevel() => CurrentLevel;
}
