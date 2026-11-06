using Godot;

public partial class StatsManager : Node
{
	[Export] public float MoveSpeed { get; set; } = 200f;
	[Export] public int CurrentLevel { get; private set; } = 1;
	public AttackSystem AttackSystem { get; private set; }
	public CriticalSystem CriticalSystem { get; private set; }
	public DefenseSystem DefenseSystem { get; private set; }


	public override void _Ready()
	{
		AttackSystem = new AttackSystem(baseDamage: 10, damagePerLevel: 3);
		CriticalSystem = new CriticalSystem(0.05f, 1.5f, 0.01f);
		DefenseSystem = new DefenseSystem(baseMaxHealth: 100f, healthPerLevel: 20f);
	}

	public void LevelUp()
	{
		CurrentLevel++;

		AttackSystem.UpdateStats(CurrentLevel);
		CriticalSystem.UpdateStats(CurrentLevel);
		DefenseSystem.UpdateStats(CurrentLevel);

		GD.Print($"Level Up {CurrentLevel}! DMG: {AttackSystem.CurrentDamage}, Crit: {CriticalSystem.CurrentCritChance * 100}%");
	}
	public int GetCurrentLevel() => CurrentLevel;
}
