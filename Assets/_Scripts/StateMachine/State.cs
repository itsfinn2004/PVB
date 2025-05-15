// Made by Niek Melet on 15/5/2025

using UnityEngine;

namespace FistFury.StateMachine
{
    public abstract class State : MonoBehaviour, IState
    {
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

        public virtual void DoBranch()
        { }

        public virtual void FixedDoBranch()
        { }

        public virtual void OnAnimatorEnter()
        { }

        public virtual void OnAnimatorEvent()
        { }

        public virtual void OnAnimatorExit()
        { }
    }
}
