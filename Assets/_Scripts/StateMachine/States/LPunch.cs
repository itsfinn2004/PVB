// Made by Finn Streunding on 16/5/2025

using UnityEngine;

namespace FistFury.StateMachine.States
{
    public class LPunch : State
    {
        [SerializeField] private AnimationClip anim;

        public override void Enter()
        {
            base.Enter();

            if(Core is playerController p)
            {
              
            }
            // play animation
            if (Animator && anim)
                Animator.Play(anim.name);
            
        }
    }
}
