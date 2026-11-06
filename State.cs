
using Godot;

public partial class State : Node
{
	[Signal]
	public delegate void TransitionRequestedEventHandler(
		State from,
		string newState
	);

	public virtual void Enter() { }
	public virtual void Exit() { }
	public virtual void HandleInput(InputEvent @event) { }
	public virtual void Update(double delta) { }
	public virtual void PhysicsUpdate(double delta) { }

	protected void ChangeState(string name)
	{
		EmitSignal(SignalName.TransitionRequested, this, name);
	}
}
