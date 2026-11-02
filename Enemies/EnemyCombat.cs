using Godot;

public partial class EnemyCombat : Node
{
	[Export] private int _damage = 10;
	[Export] private float _attackCooldown = 1.0f;

	private Area2D _hitboxArea;
	private Timer _attackTimer;
	private bool _canAttack = true;

	public override void _Ready()
	{
		var parent = GetParent();
		_hitboxArea = parent.GetNode<Area2D>("HitboxArea");
		_attackTimer = parent.GetNode<Timer>("AttackTimer");

		if (_attackTimer != null)
		{
			_attackTimer.WaitTime = _attackCooldown;
			_attackTimer.OneShot = true;
			_attackTimer.Timeout += OnCooldownFinished;
		}
	}

	public bool IsTargetInAttackRange(Node2D target)
	{
		if (_hitboxArea == null || target == null) return false;
		return _hitboxArea.GetOverlappingBodies().Contains(target);
	}

	public void TryAttack(Node2D target)
	{
		if (!_canAttack) return;

		if (target != null && target.HasMethod("TakeDamage"))
		{
			target.Call("TakeDamage", _damage);
			GD.Print($"Zadano {_damage} obrażeń graczowi!");

			_canAttack = false;
			_attackTimer.Start();
		}
	}

	private void OnCooldownFinished()
	{
		_canAttack = true;
	}
}
