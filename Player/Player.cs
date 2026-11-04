using Godot;

public partial class Player : CharacterBody2D
{
	[Export] private PlayerJump _jumpComponent;
	[Export] private PlayerMovement _movementComponent;
	[Export] private PlayerHitBox _hitboxComponent;
	[Export] private PlayerAttack _attack;
	[Export] private HealthComponent _healthComponent; // MUST be assigned in Inspector!

	private StatsManager _statsmanager;
	private LevelSystem _levelsystem;
	private PlayerAnimation _animation;
	private PlayerExp _expComponent;

	private PlayerDetails _playerDetails = new();

	public override void _Ready()
	{
		_levelsystem = GetNode<LevelSystem>("LevelSystem");
		_statsmanager = GetNode<StatsManager>("StatsManager");
		_expComponent = GetNode<PlayerExp>("PlayerExp");

		_animation = GetNode<PlayerAnimation>("PlayerAnimation");
		_attack = GetNode<PlayerAttack>("PlayerAttack");

		var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		var hitbox = GetNode<Area2D>("Hitbox");
		var hitbox2 = GetNode<Area2D>("Hitbox2");
		var hitboxShape = GetNode<CollisionShape2D>("Hitbox/CollisionShape2D");
		var hitbox2Shape = GetNode<CollisionShape2D>("Hitbox2/CollisionShape2D");

		_jumpComponent.Setup(this);
		_movementComponent.Setup(this, _playerDetails);
		_attack.Setup(this, sprite, hitbox, hitbox2, hitboxShape, hitbox2Shape, _playerDetails);
		_animation.Setup(this, sprite, _attack);

		if (_hitboxComponent != null)
			_hitboxComponent.Setup(hitboxShape, hitbox2Shape, _playerDetails);

		GD.Print($"Player._Ready: _healthComponent = {_healthComponent?.Name ?? "NULL"}");
	}

	public void KillEnemy(Enemy enemy)
	{
		if (enemy == null)
			return;

		_expComponent.AddExp(enemy.ExpReward);
		GD.Print($"Zdobyto {enemy.ExpReward} EXP!");
	}

	public void TakeDamage(int damage)
	{
		GD.Print($"Player.TakeDamage({damage}) called");

		if (_healthComponent == null)
		{
			GD.PrintErr("Player: Brak HealthComponent przypisanego w inspektorze!");
			return;
		}

		_healthComponent.TakeDamage(damage);
		GD.Print($"Gracz otrzymał {damage} obrażeń! HP: {_healthComponent.GetCurrentHealth()}/{_healthComponent.GetMaxHealth()}");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsOnFloor())
			Velocity += GetGravity() * (float)delta;

		_movementComponent.HandleMovement(delta);
		_jumpComponent.HandleJump(delta);
		_attack.HandleAttack();

		MoveAndSlide();
		_animation.UpdateAnimation();

		_hitboxComponent?.HandleHitbox();
	}
}

public interface IPlayerDirectionManagement
{
	bool HasFrontDirection { get; }
	public void SetFrontDirection();
	public void SetBackDirection();
}

public class PlayerDetails : IPlayerDirectionManagement
{
	public bool HasFrontDirection { get; private set; } = true;

	public void SetFrontDirection() => HasFrontDirection = true;
	public void SetBackDirection() => HasFrontDirection = false;
}
