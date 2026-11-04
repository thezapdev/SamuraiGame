using Godot;
using System;

public partial class HealthBarUI : ProgressBar
{
	[Export] private HealthComponent _healthComponent;

	public override void _Ready()
	{
		if (_healthComponent == null)
		{
			GD.PrintErr("HealthBarUI: Brak przypisanego HealthComponent w inspektorze!");
			return;
		}

		CallDeferred(nameof(InitializeHealth));
	}

	private void OnHealthChanged(int currentHealth, int maxHealth)
	{
		GD.Print($"HealthBarUI.OnHealthChanged: {currentHealth}/{maxHealth}");
		MaxValue = maxHealth;
		Value = currentHealth;
	}

	private void InitializeHealth()
	{
		if (_healthComponent == null) return;

		MaxValue = _healthComponent.GetMaxHealth();
		Value = _healthComponent.GetCurrentHealth();
		_healthComponent.HealthChanged += OnHealthChanged;

		GD.Print($"HealthBarUI.InitializeHealth: Max={_healthComponent.GetMaxHealth()}, Current={_healthComponent.GetCurrentHealth()}");
	}
}
