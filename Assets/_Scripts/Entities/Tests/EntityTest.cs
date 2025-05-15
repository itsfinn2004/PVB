// Made by Niek Melet on 15/5/2025

using FistFury.StateMachine.States;
using UnityEngine;

namespace FistFury.Entities.Tests
{
    public class EntityTest : Core
    {
        private StateMachine.StateMachine _stateMachine;

        [Header("Behaviors")]
        [SerializeField] private Idle idle;

        [Header("Movement")]
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
            _stateMachine = new StateMachine.StateMachine();
            _stateMachine.Initialize(this);
        }
    }
}
