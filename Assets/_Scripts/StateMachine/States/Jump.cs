// Made by Niek Melet on 15/5/2025

using FistFury.Global;
using UnityEngine;

namespace FistFury.StateMachine.States
{
    public class Jump : State
    {
        [SerializeField] private AnimationClip anim;

        public override void Enter()
        {
            base.Enter();
            
            AudioManager.Instance.Play("jump");

            // play animation
            if (Animator && anim)
                Animator.Play(anim.name);
        }
    }
}
