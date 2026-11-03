using Godot;

public partial class AttackSystem
{
    private readonly float _baseDamage;
    private readonly float _damagePerLevel;

    public int CurrentDamage { get; private set; }


    public AttackSystem(int baseDamage, float damagePerLevel)
    {
        _baseDamage = baseDamage;
        _damagePerLevel = damagePerLevel;
    }

    public void UpdateStats(int currentLevel)
    {
        CurrentDamage = (int)(_baseDamage + (_damagePerLevel * (currentLevel - 1)));
    }
}