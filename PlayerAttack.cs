using Godot;

public partial class PlayerAttack : Node
{
	[Export] private int _damage = 10;

	public bool IsAttacking { get; private set; }

	private CharacterBody2D _body;
	private AnimatedSprite2D _animatedSprite;
	private Area2D _hitbox;
	private CollisionShape2D _hitboxShape;

	public void Setup(CharacterBody2D body, AnimatedSprite2D animatedSprite, Area2D hitbox, CollisionShape2D hitboxShape)
	{
		_body = body;
		_animatedSprite = animatedSprite;
		_hitbox = hitbox;
		_hitboxShape = hitboxShape;

		_hitbox.BodyEntered += OnHitboxEntered;
		_animatedSprite.AnimationFinished += OnAnimationFinished;
	}

	public void HandleAttack()
	{
		if (IsAttacking) return;

		if (Input.IsActionJustPressed("Attack"))
		{
			IsAttacking = true;
			_hitboxShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false); // było: _hitbox
			_animatedSprite.Play("Attack");
		}
	}

	private void OnAnimationFinished()
	{
		if (_animatedSprite.Animation != "Attack") return;

		IsAttacking = false;
		_hitboxShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true); // było: false
	}

	private void OnHitboxEntered(Node2D body)
	{
		if (body is Enemy enemy)
			enemy.TakeDamage(_damage);
	}
}
