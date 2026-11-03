using Godot;

public partial class PlayerHitBox : Node
{
	private Area2D _hitbox;
	private CollisionShape2D _shape;
	private PlayerDetails _details;
	private Vector2 _baseShapePosition;

	public void Setup(Area2D area, CollisionShape2D shape, PlayerDetails details)
	{
		_hitbox = area;
		_shape = shape;
		_details = details;
		_baseShapePosition = shape.Position;
	}

	public void HandleHitbox()
	{
		float sign = _details.HasFrontDirection ? 1f : -1f;
		_shape.Position = new Vector2(Mathf.Abs(_baseShapePosition.X) * sign, _baseShapePosition.Y);
	}
}
