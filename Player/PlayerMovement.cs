
using Godot;

public partial class PlayerMovement : Node
{
	[Export] private float _speed = 200.0f;

	private IPlayerDirectionManagement _direction;
	private CharacterBody2D _body;

	public void Setup(
		CharacterBody2D body,
		IPlayerDirectionManagement direction)
	{
		_body = body;
		_direction = direction;
	}

	public void HandleMovement(double delta)
	{
		Vector2 velocity = _body.Velocity;

		float input = Input.GetAxis("ui_left", "ui_right");

		if (Mathf.Abs(input) > 0.001f)
		{
			velocity.X = input * _speed;

			if (input > 0)
				_direction.SetFrontDirection();
			else
				_direction.SetBackDirection();
		}
		else
		{
			velocity.X = Mathf.MoveToward(
				velocity.X,
				0,
				_speed * (float)delta
			);
		}

		_body.Velocity = velocity;
	}
}
