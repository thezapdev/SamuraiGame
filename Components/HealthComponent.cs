using Godot;
using System;

public partial class HealthComponent : Node
{

	[Export] public int _maxHealth;
	private int _currentHealth;

	[Signal] public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);
	[Signal] public delegate void DiedEventHandler();
	
	public override void _Ready()
	{
		
		_currentHealth = _maxHealth;
		

	}

	public void TakeDamage(int damage)
	{
		_currentHealth = Mathf.Clamp(_currentHealth - damage,0,_maxHealth);

		EmitSignal(SignalName.HealthChanged, _currentHealth, _maxHealth);

		if(_currentHealth <= 0)
		{
			EmitSignal(SignalName.Died);
		}
	}

	public void Heal(int amount)
	{
		_currentHealth = Mathf.Clamp(_currentHealth + amount,0,_maxHealth);

		EmitSignal(SignalName.HealthChanged,_currentHealth,_maxHealth);
	}

	public int GetCurrentHealth() => _currentHealth;
	public int GetMaxHealth() => _maxHealth;
}
