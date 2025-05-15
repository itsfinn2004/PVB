// Made by Niek Melet on 15/5/2025

using UnityEngine;

namespace FistFury.StateMachine
{
    public class StateMachine
    {
        public State ActiveState { get; private set; }
        public State ActiveChildState { get; private set; }

        public void Initialize()
        {
            Debug.Log("Initialized StateMachine!");
        }

        /// <summary>
        /// Changes the active state to a new state.
        /// </summary>
        /// <param name="newState">The new state to change into.</param>
        public void ChangeState(State newState)
        {
            ActiveState?.Exit();
            ActiveState = newState;

            ActiveState?.Enter();
        }

        /// <summary>
        /// Changes the active child state to a new state.
        /// </summary>
        /// <param name="newState">The new state to change the child state into.</param>
        public void ChangeChildState(State newState)
        {
            ActiveChildState?.Exit();
            ActiveChildState = newState;

            ActiveChildState?.Enter();
        }
    }
}
