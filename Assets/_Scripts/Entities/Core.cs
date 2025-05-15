// Made by Niek Melet on 15/5/2025

using System.Collections.Generic;

using UnityEngine;

using FistFury.StateMachine;

namespace FistFury.Entities
{
    /// <summary>
    /// Overall abstract class for all the entities, containing all the core data.
    /// </summary>
    public abstract class Core : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Rigidbody2D Body { get; private set; }

        protected StateMachine.StateMachine StateMachine;
        protected State CurrentState => StateMachine.CurrentState;

        protected void SetupInstances()
        {
            StateMachine = new StateMachine.StateMachine();

            // Gather all states and set their core
            State[] allChildStates = GetComponentsInChildren<State>();
            foreach (State state in allChildStates)
                state.SetCore(this);
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (Application.isPlaying && CurrentState)
            {
                List<State> states = StateMachine.GetActiveStates();
                Vector3 position = new Vector3(transform.position.x - 2, transform.position.y + 1, transform.position.z);
                UnityEditor.Handles.Label(position, "Active States: " + string.Join(" > ", states));
            }
        }

#endif
    }
}
