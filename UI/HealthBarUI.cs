using Godot;
using System;

public partial class HealthBarUI :  ProgressBar
{
	[Export] private HealthComponent _healthComponent;


	public override void _Ready()    {

		CallDeferred(nameof(InitializeHealth));

	}   

	private void OnHealthChanged(int currentHealth,int maxHealth)
	{
		MaxValue = maxHealth;
		Value = currentHealth;
	}                  
	private void InitializeHealth()
	{
		 MaxValue = _healthComponent.GetMaxHealth();
		Value = _healthComponent.GetCurrentHealth();
		_healthComponent.HealthChanged += OnHealthChanged;
			}   


}                  
