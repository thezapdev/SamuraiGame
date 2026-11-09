
using System.Collections.Generic;
using Godot;

public partial class StateMachine : Node
{
	[Export]
	public State InitialState;

	public State CurrentState { get; private set; }

	private readonly Dictionary<string, State> _states = new();

	public override async void _Ready()
	{
		foreach (Node child in GetChildren())
		{
			if (child is State state)
			{
				_states[state.Name.ToString()] = state;

				state.TransitionRequested += OnTransitionRequested;
			}
		}

		GD.Print(
			"Dostępne stany: " +
			string.Join(", ", _states.Keys)
		);

		if (InitialState == null)
		{
			GD.PushError("StateMachine: nie ustawiono InitialState!");
			return;
		}

		await ToSignal(GetParent(), Node.SignalName.Ready);

		CurrentState = InitialState;

		GD.Print($"Początkowy stan: {CurrentState.Name}");

		CurrentState.Enter();
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

	private void OnTransitionRequested(
		State from,
		string newState)
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
			GD.PushError(
				$"StateMachine: brak stanu '{newState}'!"
			);

			return;
		}

		if (next == CurrentState)
			return;

		GD.Print(
			$"Zmiana stanu: {CurrentState?.Name} -> {next.Name}"
		);

		CurrentState?.Exit();

		CurrentState = next;

		CurrentState.Enter();
	}
}
