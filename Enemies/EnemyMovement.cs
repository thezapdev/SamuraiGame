using Godot;

public partial class EnemyMovement : Node
{
	[Export] public float Speed = 150.0f;
	[Export] public float Gravity = 980.0f;
	[Export] public float TurnThreshold = 8.0f; // min. różnica X, żeby zmienić kierunek

	public int Direction { get; private set; } = 1;

	private CharacterBody2D _body;

	public override void _Ready() => _body = GetParent() as CharacterBody2D;

	[Export] public float StopDistance = 40.0f;

public void MoveTowards(float targetX)
{
	float diff = targetX - _body.GlobalPosition.X;

	if (Mathf.Abs(diff) > TurnThreshold)
		Direction = diff > 0 ? 1 : -1;

	var v = _body.Velocity;
	v.X = Mathf.Abs(diff) > StopDistance ? Direction * Speed : 0;
	_body.Velocity = v;
	GD.Print($"diff={diff:F0} StopDistance={StopDistance}");
}

	public void Stop()
	{
		var v = _body.Velocity;
		v.X = 0;
		_body.Velocity = v;
	}

	public void Move(double delta)
	{
		var v = _body.Velocity;
		v.Y = _body.IsOnFloor() ? 0 : v.Y + Gravity * (float)delta;
		_body.Velocity = v;
		_body.MoveAndSlide();
	}
}
