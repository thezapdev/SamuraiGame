using System;
using Godot;

public partial class PlayerMovement : Node
{
		[Export] private float _speed { get; set; } = 200.0f;
		
		private CharacterBody2D _body;

		public void Setup(CharacterBody2D body)
	{
		_body = body;
	}
public void HandleMovement(double delta)
	{
		Vector2 velocity = _body.Velocity;
		float direction = Input.GetAxis("ui_left","ui_right");

		if(Math.Abs(direction) > 0.001f)
		{
			velocity.X = direction*_speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(_body.Velocity.X,0,_speed);
		}
		_body.Velocity = velocity;
	}
	
}
