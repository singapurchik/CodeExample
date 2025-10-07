namespace FAS.Actors.Companion
{
	public class ChillingOnBed : CompanionState
	{
		public override GirlStates Key => GirlStates.ChillingOnBed;
		
		public override void Enter()
		{
			Animator.PlayChillingOnBedAnim();
		}
	}
}