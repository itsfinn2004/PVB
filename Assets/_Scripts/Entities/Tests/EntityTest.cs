// Made by Niek Melet on 15/5/2025

using UnityEngine;

using FistFury.StateMachine;
using FistFury.StateMachine.States;

namespace FistFury.Entities.Tests
{
    public class EntityTest : Core
    {
        [Header("Behaviors")]
        [SerializeField] private Idle idle;

        [Space()]
        [SerializeField] private Move move;
        [SerializeField] private Jump jump;

#if UNITY_EDITOR

        private void OnValidate()
        {
            idle = GetComponentInChildren<Idle>();
            move = GetComponentInChildren<Move>();
            jump = GetComponentInChildren<Jump>();
        }

#endif

        private void Start()
        {
            SetupInstances();
            StateMachine.Set(idle);
        }

        private void Update()
        {
            SelectState();
        }

        private void SelectState()
        {
            State oldState = StateMachine.CurrentState;
            State newState;

            if (UnityEngine.Input.GetKey(KeyCode.A))
                newState = move;
            else if (UnityEngine.Input.GetKey(KeyCode.Space))
                newState = jump;
            else
                newState = idle;

            // only change if the new state is different
            if (newState && newState != oldState)
                StateMachine.Set(newState);
        }
    }
}
