// Made by Niek Melet on 15/5/2025

using FistFury.Entities;
using UnityEngine;

namespace FistFury.StateMachine
{
    public class StateMachine
    {
        public State ActiveState { get; private set; }
        public State ActiveChildState { get; private set; }

        private Core _core;

        public void Initialize(Core core)
        {
            _core = core;
        }

        /// <summary>
        /// Changes the active state to a new state.
        /// </summary>
        /// <param name="newState">The new state to change into.</param>
        public void ChangeState(State newState)
        {
            ActiveState?.Exit();
            ActiveState = newState;

            ActiveState?.Initialize(_core);
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

            ActiveChildState?.Initialize(_core);
            ActiveChildState?.Enter();
        }
    }
}
