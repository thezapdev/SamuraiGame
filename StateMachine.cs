using Godot;
using System.Collections.Generic;

public partial class StateMachine : Node
{
	[Export] public State InitialState;

	public State Current { get; private set; }
	private readonly Dictionary<string, State> _states = new();

	public override void _Ready()
	{
		foreach (var child in GetChildren())
		{
			if (child is State state)
			{
				_states[state.Name] = state;
				state.Transitioned += OnTransitioned;
			}
		}

		CallDeferred(nameof(Start));
	}

	private void Start()
	{
		Current = InitialState;
		GD.Print($"StateMachine start, Initial = {InitialState?.Name}");
		Current?.Enter();
	}

	public override void _Process(double delta) => Current?.Update(delta);
	public override void _PhysicsProcess(double delta)
	{
		Current?.PhysicsUpdate(delta);


	}

	private void OnTransitioned(State from, string newStateName)
	{
		if (from != Current) return;
		SwitchTo(newStateName);
	}

	public void ForceTransition(string newStateName) => SwitchTo(newStateName);

	private void SwitchTo(string name)
	{
		if (!_states.TryGetValue(name, out var next))
		{
			GD.PushError($"StateMachine: brak stanu {name}");
			return;
		}
		if (Current == next) return;

		Current?.Exit();
		Current = next;
		Current.Enter();
	}

}
