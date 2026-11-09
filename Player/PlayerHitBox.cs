
using Godot;

public partial class PlayerHitBox : Node
{
	private CollisionShape2D _shapeRight;
	private CollisionShape2D _shapeLeft;

	private PlayerDetails _details;

	public void Setup(
		CollisionShape2D shapeRight,
		CollisionShape2D shapeLeft,
		PlayerDetails details)
	{
		_shapeRight = shapeRight;
		_shapeLeft = shapeLeft;
		_details = details;
	}
}
