using FAS.Players.States;

namespace FAS.Players
{
	public interface IPlayerStateMachineInfo : IStateMachineInfo<PlayerStates>
	{
        public IPrepareInteractionState CurrentPrepareInteractionState { get; }
	}
}