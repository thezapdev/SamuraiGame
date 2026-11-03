using Godot;

public partial class CriticalSystem
{
    private readonly float _baseCritChance;
    private readonly float _baseCritMultiplier;
    private readonly float _critChancePerLevel;

    public float CurrentCritChance { get; private set; }
    public float CritMultiplier => _baseCritMultiplier;

    public CriticalSystem(float baseCritChance = 0.05f, float baseCritMultiplier = 1.5f, float critChancePerLevel = 0.01f)
    {
        _baseCritChance = baseCritChance;
        _baseCritMultiplier = baseCritMultiplier;
        _critChancePerLevel = critChancePerLevel;
        UpdateStats(1);
    }

    public void UpdateStats(int level)
    {
        CurrentCritChance = Mathf.Clamp(_baseCritChance + _critChancePerLevel * (level - 1), 0f, 1f);
    }

    public int ApplyCrit(int damage)
    {
        if (GD.Randf() < CurrentCritChance)
            return Mathf.RoundToInt(damage * CritMultiplier);

        return damage;
    }
}