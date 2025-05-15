// Made by Niek Melet on 15/5/2025

namespace FistFury.StateMachine
{
    public interface IState
    {
        void Enter();
        void Exit();

        void Do();
        void FixedDo();

        void DoBranch();
        void FixedDoBranch();

        // Gets called on the animations
        void OnAnimatorEnter();
        void OnAnimatorEvent();
        void OnAnimatorExit();
    }
}
