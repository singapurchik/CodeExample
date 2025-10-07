using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace FAS
{
    public abstract class StateMachine<TEnum, TState> : MonoBehaviour,
	    IStateMachineInfo<TEnum>
        where TEnum : struct, Enum
        where TState : State<TEnum, TEnum>
    {
		[Inject] private IPausableInfo _pausable;
	    
        private readonly Dictionary<TEnum, TState> _states = new (20);
        
        protected TState CurrentState;
        protected TState LastState;

        private TState _requestedState;
        
        public TEnum CurrentStateKey => CurrentState.Key;
        public TEnum LastStateKey => LastState.Key;
        
        public abstract void Initialize();

        public bool TryGetLastStateKey(out TEnum key)
        {
	        if (LastState != null)
	        {
		        key = LastState.Key;
		        return true;
	        }

	        key = default;
	        return false;
        }

        protected virtual void OnDisable()
        {
	        if (CurrentState != null)
		        CurrentState.Exit();
        }
        
        protected void AddState(TState state)
        {
	        _states.Add(state.Key, state);
        }

        public void Request(TEnum key) => _requestedState = _states[key];

        public void TryReturnLastState()
        {
            if (LastState != null)
	            _requestedState = LastState;
        }
        
        public virtual void TrySwitchStateTo(TEnum stateKey) => TrySwitchStateTo(_states[stateKey]);
        
        protected virtual void TrySwitchStateTo(TState nextState)
        {
	        if (nextState != CurrentState)
		        SwitchStateTo(nextState);
        }
        
        protected void SwitchStateTo(TState state)
        {
	        if (CurrentState != null)
		        ExitCurrentState();

	        CurrentState = state;
	        CurrentState.Enter();
        }

        protected virtual void ExitCurrentState()
        {
	        LastState = CurrentState;
	        CurrentState.Exit();
        }

        protected virtual void Update()
        {
	        if (_pausable == null || _pausable.IsPaused) return;
	        
	        CurrentState?.Perform();

            if (_requestedState != null)
            {
	            TrySwitchStateTo(_requestedState);
                _requestedState = null;
            }
            else if (CurrentState != null && CurrentState.IsReadyToTransit)
            {
	            TrySwitchStateTo(_states[CurrentState.NextStateKey]);
            }
        }
    }
}