using Godot;
public partial class DefenseSystem
{
    private readonly float _baseMaxHealth;
    private readonly float _healthPerLevel;

    public float CurrentMaxHealth { get; private set; }

    public DefenseSystem(float baseMaxHealth, float healthPerLevel)
    {
        _baseMaxHealth = baseMaxHealth;
        _healthPerLevel = healthPerLevel;
        UpdateStats(1);
    }

    public void UpdateStats(int currentLevel)
    {
        CurrentMaxHealth = _baseMaxHealth + (_healthPerLevel * (currentLevel - 1));
    }
}
