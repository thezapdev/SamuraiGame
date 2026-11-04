using Godot;

public partial class EnemyMovement : Node
{
	[Export] private float _speed = 180.0f;
	[Export] private float _gravity = 980.0f;
	[Export] private float _stopDistance = 20.0f;
	[Export] private float _detectionRange = 0.0f; // 0 = goni zawsze
	[Export] private bool _debug = false;

	private CharacterBody2D _body;
	private Node2D _target;

	public override void _Ready()
	{
		_body = GetParent<CharacterBody2D>();
		if (_body == null)
		{
			GD.PrintErr($"EnemyMovement [{Name}]: Rodzic nie jest CharacterBody2D!");
			return;
		}

		_target = GetTree().GetFirstNodeInGroup("Player") as Node2D;
		if (_target == null)
		{
			GD.PrintErr($"EnemyMovement [{Name}]: Nie znaleziono gracza w grupie 'Player'!");
		}
		else if (_debug)
		{
			GD.Print($"EnemyMovement [{Name}]: Cel znaleziony: {_target.Name}");
		}
	}

	public void HandleMovement(double delta)
	{
		if (_body == null || _target == null)
			return;

		Vector2 targetPos = _target.GlobalPosition;
		Vector2 myPos = _body.GlobalPosition;

		float distX = Mathf.Abs(targetPos.X - myPos.X);
		float dirX = Mathf.Sign(targetPos.X - myPos.X);
		float distance = myPos.DistanceTo(targetPos);

		Vector2 velocity = _body.Velocity;

		bool outOfRange = _detectionRange > 0 && distance > _detectionRange;

		if (outOfRange || distX <= _stopDistance)
		{
			// Stój (hamuj w poziomie)
			velocity.X = Mathf.MoveToward(velocity.X, 0, _speed);
		}
		else
		{
			velocity.X = dirX * _speed;
		}

		// Grawitacja
		if (!_body.IsOnFloor())
			velocity.Y += _gravity * (float)delta;

		_body.Velocity = velocity;
		_body.MoveAndSlide();

		if (_debug)
			GD.Print($"EnemyMovement [{Name}] | distX={distX:F1} vel={_body.Velocity}");
	}
}
