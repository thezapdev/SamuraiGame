using Godot;

public partial class EnemyCombat : Node
{
	[Export] private int _damage = 10;
	[Export] private float _attackCooldown = 1.5f;
	[Export] private float _attackWindup = 0.5f; // czas trwania animacji ataku

	private Area2D _hitboxArea;
	private Timer _attackTimer;
	private AnimatedSprite2D _enemySprite;
	private bool _canAttack = true;
	private Node2D _target;

	private bool _isWindingUp = false;
	private float _windupTimeLeft = 0f;

	public override void _Ready()
	{
		var parent = GetParent();
		if (parent != null)
		{
			_hitboxArea = parent.GetNodeOrNull<Area2D>("HitboxArea");
			_attackTimer = parent.GetNodeOrNull<Timer>("AttackTimer");
			_enemySprite = parent.GetNodeOrNull<AnimatedSprite2D>("EnemySprite2D");
		}

		if (_attackTimer != null)
		{
			_attackTimer.WaitTime = _attackCooldown;
			_attackTimer.OneShot = true;
			_attackTimer.Timeout += OnCooldownFinished;
		}

		_target = GetTree().GetFirstNodeInGroup("Player") as Node2D;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_target == null)
		{
			_target = GetTree().GetFirstNodeInGroup("Player") as Node2D;
			return;
		}

		// Jeśli trwa animacja ataku – odliczaj czas
		if (_isWindingUp)
		{
			_windupTimeLeft -= (float)delta;
			if (_windupTimeLeft <= 0f)
			{
				_isWindingUp = false;

				// Po zakończeniu animacji sprawdź, czy gracz wciąż w zasięgu
				if (_hitboxArea != null && IsTargetInAttackRange(_target))
				{
					DealDamage(_target);
				}

				// Uruchom cooldown przed następnym atakiem
				if (_attackTimer != null)
					_attackTimer.Start();
				else
					_canAttack = true;
			}
			return;
		}

		// Gracz w zasięgu i możemy zacząć atak
		if (_canAttack && _hitboxArea != null && IsTargetInAttackRange(_target))
		{
			StartAttack();
		}
	}

	private void StartAttack()
	{
		_canAttack = false;
		_isWindingUp = true;
		_windupTimeLeft = _attackWindup;

		if (_enemySprite != null)
		{
			_enemySprite.Play("Attack");
		}

		GD.Print($"EnemyCombat: Atak trwa {_attackWindup}s – gracz może uciec!");
	}

	private void DealDamage(Node2D target)
	{
		if (target == null) return;

		if (target.HasMethod("TakeDamage"))
		{
			target.Call("TakeDamage", _damage);
			GD.Print($"Zadano {_damage} obrażeń graczowi!");
		}
	}

	private void OnCooldownFinished()
	{
		_canAttack = true;
	}

	public bool IsTargetInAttackRange(Node2D target)
	{
		if (_hitboxArea == null || target == null) return false;
		return _hitboxArea.GetOverlappingBodies().Contains(target);
	}
}
