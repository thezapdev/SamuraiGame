using Godot;

public partial class PlayerAttack : Node
{
	[Export] private StatsManager _statsManager;
	private AttackSystem _attacksystem;
	private CriticalSystem _criticalSystem;

	public bool IsAttacking { get; private set; }

	private CharacterBody2D _body;
	private AnimatedSprite2D _animatedSprite;
	private Area2D _hitbox;
	private Area2D _hitbox2;
	private CollisionShape2D _hitboxShape;
	private CollisionShape2D _hitbox2Shape;
	private IPlayerDirectionManagement _direction;
	
	public override void _Ready()
	{
		_attacksystem = _statsManager.AttackSystem;
		_attacksystem.UpdateStats(1);
		_criticalSystem = _statsManager.CriticalSystem;
	}

	public void Setup(CharacterBody2D body, AnimatedSprite2D animatedSprite, Area2D hitbox, Area2D hitbox2,
		CollisionShape2D hitboxShape, CollisionShape2D hitbox2Shape, IPlayerDirectionManagement direction)
	{
		_body = body;
		_animatedSprite = animatedSprite;
		_hitbox = hitbox;
		_hitbox2 = hitbox2;
		_hitboxShape = hitboxShape;
		_hitbox2Shape = hitbox2Shape;
		_direction = direction;

		if (_hitbox != null)
			_hitbox.BodyEntered += OnHitboxEntered;
		if (_hitbox2 != null)
			_hitbox2.BodyEntered += OnHitboxEntered;

		if (_animatedSprite != null)
			_animatedSprite.AnimationFinished += OnAnimationFinished;
	}

	public void OnLevelChanged(int newLevel)
	{
		_attacksystem?.UpdateStats(newLevel);
		_criticalSystem?.UpdateStats(newLevel);
	}

	public void HandleAttack()
	{
		if (IsAttacking) return;

		if (Input.IsActionJustPressed("Attack"))
		{
			IsAttacking = true;

			if (_direction != null && _direction.HasFrontDirection) // patrzy w prawo
			{
				if (_hitboxShape != null)
					_hitboxShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
				if (_hitbox2Shape != null)
					_hitbox2Shape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			}
			else // patrzy w lewo
			{
				if (_hitboxShape != null)
					_hitboxShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
				if (_hitbox2Shape != null)
					_hitbox2Shape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
			}

			if (_animatedSprite != null)
				_animatedSprite.Play("Attack");
		}
	}

	private void OnAnimationFinished()
	{
		if (_animatedSprite == null) return;
		if (_animatedSprite.Animation != "Attack") return;

		IsAttacking = false;

		if (_hitboxShape != null)
			_hitboxShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		if (_hitbox2Shape != null)
			_hitbox2Shape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	private void OnHitboxEntered(Node2D body)
	{
		if (body is Enemy enemy)
		{
			int damage = _criticalSystem?.ApplyCrit(_attacksystem?.CurrentDamage ?? 0) ?? 0;
			enemy.TakeDamage(damage);
		}
	}
}
