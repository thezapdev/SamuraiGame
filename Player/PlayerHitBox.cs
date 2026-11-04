using Godot;

public partial class PlayerHitBox : Node
{
	private CollisionShape2D _shapeRight;
	private CollisionShape2D _shapeLeft;
	private PlayerDetails _details;

	public void Setup(CollisionShape2D shapeRight, CollisionShape2D shapeLeft, PlayerDetails details)
	{
		_shapeRight = shapeRight;
		_shapeLeft = shapeLeft;
		_details = details;
	}

	public void HandleHitbox()
	{
		if (_details == null)
			return;

		if (_details.HasFrontDirection) // gracz patrzy w prawo → wyłącz lewy
		{
			_shapeLeft?.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		}
		else // gracz patrzy w lewo → wyłącz prawy
		{
			_shapeRight?.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		}
	}
}
