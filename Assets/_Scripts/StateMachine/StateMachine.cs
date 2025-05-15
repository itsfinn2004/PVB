// Made by Niek Melet on 15/5/2025

using System.Collections.Generic;

namespace FistFury.StateMachine
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }

        public void Set(State newState, bool forceReset = false)
        {
            if (CurrentState != newState || forceReset)
            {
                CurrentState?.Exit();
                CurrentState = newState;
                CurrentState.Initialise(this);
                CurrentState.Enter();
            }
        }

        public List<State> GetActiveStates(List<State> list = null)
        {
            // initialize list if null
            list ??= new List<State>();

            // return list if there's no active state
            if (!CurrentState)
                return list;

            // add the current state
            list.Add(CurrentState);
            CurrentState.ChildStateMachine?.GetActiveStates(list);

            return list;
        }
    }
}
