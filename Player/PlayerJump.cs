using System;
using Godot;

public partial class PlayerJump : Node
{
	[Export] private float _jumpVelocity = -300f;
	[Export] private float _gravityMultiplier = 1.0f;
	private CharacterBody2D _body;

public void Setup(CharacterBody2D body)
	{
		_body = body;
	}
public void HandleJump(double delta)
	{
		if (_body == null) return;

		if (Input.IsActionJustPressed("ui_accept") && _body.IsOnFloor())
		{
			Vector2 velocity = _body.Velocity;
			velocity.Y = _jumpVelocity;
			_body.Velocity = velocity;
		}
		if (Input.IsActionJustReleased("ui_accept") && _body.Velocity.Y < 0)
		{
			_body.Velocity = new Vector2(_body.Velocity.X, _body.Velocity.Y * 0.5f);
		}
	}
}
