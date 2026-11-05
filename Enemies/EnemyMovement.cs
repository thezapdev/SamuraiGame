
using Godot;

public partial class EnemyMovement : Node
{
	[Export] public float Speed = 180.0f;
	[Export] public float Gravity = 980.0f;

	private CharacterBody2D _enemy;
	private Node2D _player;

	public override void _Ready()
	{
		_enemy = GetParent() as CharacterBody2D;

		if (_enemy == null)
		{
			GD.PrintErr("EnemyMovement: Nie znaleziono Enemy!");
			return;
		}
	}

	public void SetTarget(Node2D target)
	{
		_player = target;
	}

	public void HandleMovement(double delta)
	{
		if (_enemy == null || !GodotObject.IsInstanceValid(_player))
			return;

		float enemyX = _enemy.GlobalPosition.X;
		float playerX = _player.GlobalPosition.X;

		float differenceX = playerX - enemyX;
		float direction = Mathf.Sign(differenceX);

		Vector2 velocity = _enemy.Velocity;

		velocity.X = Mathf.Abs(differenceX) > 5.0f
			? direction * Speed
			: 0;

		if (!_enemy.IsOnFloor())
			velocity.Y += Gravity * (float)delta;
		else
			velocity.Y = 0;

		GD.Print(
			$"EnemyX: {enemyX:F1} | " +
			$"PlayerX: {playerX:F1} | " +
			$"Difference: {differenceX:F1} | " +
			$"Direction: {direction}"
		);

		_enemy.Velocity = velocity;
		_enemy.MoveAndSlide();
	}
}
