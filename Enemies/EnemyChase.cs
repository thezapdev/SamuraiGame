using Godot;

public partial class EnemyChase : EnemyState
{
	public override void Enter() => GD.Print("Chase start");

	
	public override void PhysicsUpdate(double delta) => ChangeState("Chase");


}
