using Godot;

public partial class TestEnemy : CharacterBody2D
{
	[Export] public float Speed = 150.0f;
	[Export] public float Gravity = 980.0f;

	private Node2D _target;

	public override void _Ready()
	{
		_target = GetTree().GetFirstNodeInGroup("Player") as Node2D;
		GD.Print($"TestEnemy target={_target?.Name}");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_target == null) return;

		float dirX = Mathf.Sign(_target.GlobalPosition.X - GlobalPosition.X);

		Vector2 velocity = Velocity;
		velocity.X = dirX * Speed;

		if (!IsOnFloor())
			velocity.Y += Gravity * (float)delta;

		Velocity = velocity;
		MoveAndSlide();

		GD.Print($"pos={GlobalPosition} tgt={_target.GlobalPosition} dirX={dirX} vel={Velocity} floor={IsOnFloor()}");
	}
}
