using Godot;

public partial class PlayerAnimation : Node
{
	private CharacterBody2D _body;
	private AnimatedSprite2D _sprite;
	private PlayerAttack _attack;

	public void Setup(CharacterBody2D body, AnimatedSprite2D sprite, PlayerAttack attack)
	{
		_body = body;
		_sprite = sprite;
		_attack = attack;
	}

	public void UpdateAnimation()
	{
		if (_attack.IsAttacking) return;

		if (_body.Velocity.X < 0) _sprite.FlipH = true;
		else if (_body.Velocity.X > 0) _sprite.FlipH = false;

		Play(Mathf.Abs(_body.Velocity.X) > 0.1f ? "Run" : "Idle");
	}

	private void Play(string name)
	{
		if (_sprite.Animation != name || !_sprite.IsPlaying())
			_sprite.Play(name);
	}
}
