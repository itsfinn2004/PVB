// Made by Niek Melet on 15/5/2025

using FistFury.Entities;
using UnityEngine;

namespace FistFury.StateMachine
{
    public abstract class State : MonoBehaviour, IState
    {
        public bool IsComplete { get; protected set; }

        // keep track of how long this state is active
        protected float StartTime;
        protected float RunTime => Time.time - StartTime;

        // blackboard
        [field: SerializeField] protected Core Core { get; private set; }
        protected Animator Animator => Core.Animator;
        protected Rigidbody2D Body => Core.Body;

        public StateMachine ChildStateMachine { get; private set; }
        protected StateMachine ParentStateMachine { get; private set; }
        public State ActiveState => ChildStateMachine.CurrentState;

        public void Initialise(StateMachine parent)
        {
            ParentStateMachine = parent;
            IsComplete = false;
            StartTime = Time.time;
        }

        protected void Set(State newState, bool forceReset = false)
        {
            ChildStateMachine.Set(newState, forceReset);
        }

        public void SetCore(Core core)
        {
            Core = core;
        }

        #region State Methods

        public virtual void Enter()
        {
            Debug.Log($"Entered {GetType()} State!");
        }

        public virtual void Exit()
        {
            Debug.Log($"Exited out of {GetType()} State!");
        }

        public virtual void Do()
        { }

        public virtual void FixedDo()
        { }

        // calls to get down to the children
        public virtual void DoBranch()
        {
            Do();
            ActiveState?.DoBranch();
        }

        public void FixedDoBranch()
        {
            FixedDo();
            ActiveState?.FixedDo();
        }

        public void OnAnimatorEnter()
        { }

        public virtual void OnAnimatorEvent()
        { }

        public virtual void OnAnimatorExit()
        { }

        #endregion

    }
}
