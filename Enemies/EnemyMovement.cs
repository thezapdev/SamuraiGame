using Godot;

public partial class EnemyMovement : Node
{
	[Export] private float _speed = 150.0f;
	private CharacterBody2D _body;

	public override void _Ready()
	{
		_body = GetParent<CharacterBody2D>();
	}

	public void MoveTowards(Vector2 targetPosition)
	{
		if (_body == null) return;

		Vector2 direction = (targetPosition - _body.GlobalPosition).Normalized();
		_body.Velocity = direction * _speed;
		_body.MoveAndSlide();
	}
}
