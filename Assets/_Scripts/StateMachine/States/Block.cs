//Gemaakt door finn streunding op 16 mei 2025

using UnityEngine;

namespace FistFury.StateMachine.States
{
    public class Block : State
    {
        [SerializeField] private AnimationClip anim;
        [SerializeField] private GameObject shield;

        public override void Enter()
        {
            base.Enter();

            // play animation
            if (Animator && anim)
                Animator.Play(anim.name);
        }
        public override void Exit()
        {
            base.Exit();
            shield.SetActive(false);
        }
    }
}
