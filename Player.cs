using Godot;

public partial class Player : CharacterBody2D
{
	[Export] private PlayerJump _jumpComponent;
	[Export] private PlayerMovement _movementComponent;

	private PlayerAnimation _animation;
	private PlayerAttack _attack;

	public override void _Ready()
	{
		_animation = GetNode<PlayerAnimation>("PlayerAnimation");
		_attack = GetNode<PlayerAttack>("PlayerAttack");

		var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		var hitbox = GetNode<Area2D>("Hitbox");
		var hitboxShape = GetNode<CollisionShape2D>("Hitbox/CollisionShape2D");

		_jumpComponent.Setup(this);
		_movementComponent.Setup(this);
		_attack.Setup(this, sprite, hitbox, hitboxShape);
		_animation.Setup(this, sprite, _attack);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsOnFloor())
		{
			Velocity += GetGravity() * (float)delta;
		}

		_movementComponent.HandleMovement(delta);
		_jumpComponent.HandleJump(delta);
		_attack.HandleAttack();

		MoveAndSlide();
		_animation.UpdateAnimation();
	}
}
