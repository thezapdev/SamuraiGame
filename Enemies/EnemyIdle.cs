using Godot;

public partial class EnemyIdle : EnemyState
{
	public override void PhysicsUpdate(double delta) => ChangeState("Chase");
}
