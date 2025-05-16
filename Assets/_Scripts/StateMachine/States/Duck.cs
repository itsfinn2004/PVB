// Made by Finn Streunding on 15/5/2025

using UnityEngine;

namespace FistFury.StateMachine.States
{
    public class Duck : State
    {
        [SerializeField] private AnimationClip anim;

        public override void Enter()
        {
            base.Enter();

            // play animation
            if (Animator && anim)
                Animator.Play(anim.name);
        }
    }
}
