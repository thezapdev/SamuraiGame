
using System.Collections.Generic;
using Godot;

public partial class StateMachine : Node
{
	[Export] public State InitialState;

	public State CurrentState { get; private set; }

	private readonly Dictionary<string, State> _states = new();

	public override async void _Ready()
	{
		// StateMachine wykonuje fizykę przed Player.
		ProcessPhysicsPriority = -1;

		_states.Clear();

		foreach (Node child in GetChildren())
		{
			if (child is State state)
			{
				string stateName = state.Name.ToString();

				_states[stateName] = state;

				state.TransitionRequested += OnTransitionRequested;
			}
		}

		if (InitialState == null)
		{
			GD.PushError("StateMachine: Nie ustawiono InitialState!");
			return;
		}

		if (!_states.ContainsKey(InitialState.Name.ToString()))
		{
			GD.PushError("StateMachine: InitialState nie jest dzieckiem StateMachine!");
			return;
		}

		await ToSignal(GetParent(), Node.SignalName.Ready);

		if (!IsInsideTree())
			return;

		CurrentState = InitialState;
		CurrentState.Enter();

		GD.Print($"StateMachine: rozpoczęto {CurrentState.Name}");
	}

	public override void _UnhandledInput(InputEvent e)
	{
		CurrentState?.HandleInput(e);
	}

	public override void _Process(double delta)
	{
		CurrentState?.Update(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		CurrentState?.PhysicsUpdate(delta);
	}

	private void OnTransitionRequested(State from, string newState)
	{
		if (from != CurrentState)
			return;

		TransitionTo(newState);
	}

	public void ForceTransition(string newState)
	{
		TransitionTo(newState);
	}

	private void TransitionTo(string newState)
	{
		if (!_states.TryGetValue(newState, out State next))
		{
			GD.PushError($"StateMachine: Nie znaleziono stanu '{newState}'!");
			return;
		}

		if (next == CurrentState)
			return;

		GD.Print($"StateMachine: {CurrentState?.Name} -> {next.Name}");

		CurrentState?.Exit();

		CurrentState = next;

		CurrentState.Enter();
	}
}
