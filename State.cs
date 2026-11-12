using Godot;

public partial class State : Node
{
	[Signal] public delegate void TransitionedEventHandler(State from, string newStateName);

	public virtual void Enter() { }
	public virtual void Exit() { }
	public virtual void Update(double delta) { }
	public virtual void PhysicsUpdate(double delta) { }

	protected void ChangeState(string stateName)
		=> EmitSignal(SignalName.Transitioned, this, stateName);
}
