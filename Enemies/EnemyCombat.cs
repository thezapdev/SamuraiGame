using Godot;

public partial class EnemyCombat : Node
{
	[Signal] public delegate void AttackFinishedEventHandler();

	[Export] private int _damage = 10;
	[Export] private float _attackCooldown = 1.5f;
	[Export] private float _attackWindup = 0.5f;

	private Area2D _hitboxArea;
	private AnimatedSprite2D _enemySprite;
	private Node2D _target;

	private bool _isAttacking;
	private double _cooldownLeft;

	public bool IsAttacking => _isAttacking;

	public override void _Ready()
	{
		var parent = GetParent();
		_hitboxArea = parent.GetNodeOrNull<Area2D>("HitboxArea");
		_enemySprite = parent.GetNodeOrNull<AnimatedSprite2D>("EnemySprite2D");
		_target = GetTree().GetFirstNodeInGroup("Player") as Node2D;
	}

	public override void _Process(double delta)
	{
		if (_cooldownLeft > 0)
			_cooldownLeft -= delta;
	}

	public bool CanAttack()
		=> !_isAttacking && _cooldownLeft <= 0 && IsTargetInAttackRange();

	public bool IsTargetInAttackRange()
	{
		if (_hitboxArea == null || !GodotObject.IsInstanceValid(_target))
			return false;

		return _hitboxArea.GetOverlappingBodies().Contains(_target);
	}

	public async void StartAttack()
	{
		if (_isAttacking) return;

		_isAttacking = true;
		_enemySprite?.Play("Attack");

		await ToSignal(GetTree().CreateTimer(_attackWindup), SceneTreeTimer.SignalName.Timeout);

		// Wróg mógł zginąć albo dostać hurt w trakcie windupu
		if (!_isAttacking || !IsInstanceValid(this))
			return;

		if (IsTargetInAttackRange())
			DealDamage();

		_isAttacking = false;
		_cooldownLeft = _attackCooldown;
		EmitSignal(SignalName.AttackFinished);
	}

	// Wołane przez stan Hurt/Dead, gdy atak ma zostać przerwany
	public void CancelAttack()
	{
		_isAttacking = false;
		_cooldownLeft = _attackCooldown;
	}

	private void DealDamage()
	{
		if (_target.HasMethod("TakeDamage"))
			_target.Call("TakeDamage", _damage);
	}
}
