
using Godot;

public partial class Player : CharacterBody2D
{
	[Export] private HealthComponent _healthComponent;
	[Export] private PlayerHitBox _hitboxComponent;

	public StateMachine StateMachine { get; private set; }
	public AnimatedSprite2D Sprite { get; private set; }
	public Area2D Hitbox { get; private set; }
	public Area2D Hitbox2 { get; private set; }
	public CollisionShape2D HitboxShape { get; private set; }
	public CollisionShape2D Hitbox2Shape { get; private set; }
	public StatsManager Stats { get; private set; }
	public PlayerDetails Details { get; } = new();

	public bool FacingRight => Details.HasFrontDirection;

	private LevelSystem _levelSystem;

	public override void _Ready()
	{
		// Gracz wykonuje fizykę po StateMachine.
		ProcessPhysicsPriority = 1;

		_levelSystem = GetNode<LevelSystem>("LevelSystem");
		Stats = GetNode<StatsManager>("StatsManager");
		StateMachine = GetNode<StateMachine>("StateMachine");

		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		Hitbox = GetNode<Area2D>("Hitbox");
		Hitbox2 = GetNode<Area2D>("Hitbox2");

		HitboxShape = GetNode<CollisionShape2D>(
			"Hitbox/CollisionShape2D"
		);

		Hitbox2Shape = GetNode<CollisionShape2D>(
			"Hitbox2/CollisionShape2D"
		);

		HitboxShape.SetDeferred(
			CollisionShape2D.PropertyName.Disabled, true
		);

		Hitbox2Shape.SetDeferred(
			CollisionShape2D.PropertyName.Disabled, true
		);

		if (_hitboxComponent != null)
		{
			_hitboxComponent.Setup(
				HitboxShape,
				Hitbox2Shape,
				Details
			);
		}

		GD.Print($"Player initialized. MoveSpeed: {Stats.MoveSpeed}");
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();

		_hitboxComponent?.HandleHitbox();
	}

	public void ApplyGravity(double delta)
	{
		if (!IsOnFloor())
		{
			Velocity += GetGravity() * (float)delta;
		}
	}

	public float GetInputDirection()
	{
		return Input.GetAxis("move_left", "move_right");
	}

	public void FaceDirection(float dir)
	{
		if (dir == 0)
			return;

		if (dir > 0)
			Details.SetFrontDirection();
		else
			Details.SetBackDirection();

		Sprite.FlipH = !FacingRight;
	}

	public void KillEnemy(Enemy enemy)
	{
		if (enemy == null)
			return;

		_levelSystem.AddExp(enemy.ExpReward);

		GD.Print($"Zdobyto {enemy.ExpReward} EXP!");
	}

	public void TakeDamage(int damage)
	{
		if (_healthComponent == null)
		{
			GD.PrintErr("Player: Brak HealthComponent!");
			return;
		}

		_healthComponent.TakeDamage(damage);

		GD.Print(
			$"Gracz otrzymał {damage} obrażeń! " +
			$"HP: {_healthComponent.GetCurrentHealth()}/" +
			$"{_healthComponent.GetMaxHealth()}"
		);
	}
}

public interface IPlayerDirectionManagement
{
	bool HasFrontDirection { get; }

	void SetFrontDirection();
	void SetBackDirection();
}

public class PlayerDetails : IPlayerDirectionManagement
{
	public bool HasFrontDirection { get; private set; } = true;

	public void SetFrontDirection() => HasFrontDirection = true;

	public void SetBackDirection() => HasFrontDirection = false;
}
