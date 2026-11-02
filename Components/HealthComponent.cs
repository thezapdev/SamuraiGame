using Godot;

public partial class HealthComponent : Node
{
	[Export] public int _maxHealth = 100; // Domyślna wartość, żeby nie było 0!
	private int _currentHealth;

	[Signal] public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);
	[Signal] public delegate void DiedEventHandler();
	
	public override void _Ready()
	{
		// Ustawiamy zdrowie na samym początku
		_currentHealth = _maxHealth;
		
		// Opcjonalnie: możemy dać małe opóźnienie lub wysłać stan początkowy, 
		// ale uwaga – nie wywołujemy sygnału Died przy starcie!
	}

	public void TakeDamage(int damage)
	{
		if (_currentHealth <= 0) return; // Jeśli już martwy, nie przyjmuj obrażeń

		_currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);

		EmitSignal(SignalName.HealthChanged, _currentHealth, _maxHealth);

		if (_currentHealth <= 0)
		{
			EmitSignal(SignalName.Died);
		}
	}

	public void Heal(int amount)
	{
		if (_currentHealth <= 0) return;

		_currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);

		EmitSignal(SignalName.HealthChanged, _currentHealth, _maxHealth);
	}

	public int GetCurrentHealth() => _currentHealth;
	public int GetMaxHealth() => _maxHealth;
}
