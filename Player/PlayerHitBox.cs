using Godot;

public partial class PlayerHitBox : Node
{
	private CollisionShape2D _shape;
	private PlayerDetails _details;
	private float _baseX;

	public void Setup(Area2D area, CollisionShape2D shape, PlayerDetails details)
	{
		_shape = shape;
		_details = details;
		_baseX = shape.Position.X; // bez Abs: bierzemy wartość z edytora taką, jaka jest
	}

	public void HandleHitbox()
	{
		float sign = _details.HasFrontDirection ? 1f : -1f;
		_shape.Position = new Vector2(_baseX * sign, _shape.Position.Y);
	}
}
