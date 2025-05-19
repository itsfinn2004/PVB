// Made by Niek Melet on 15/5/2025

using UnityEngine;

namespace FistFury.StateMachine.States
{
    public class Move : State
    {
        [SerializeField] private AnimationClip anim;

        public override void Enter()
        {
            base.Enter();
            Debug.Log(anim);
            Debug.Log(Animator);
            // play animation
            if (Animator && anim)
            {
                Debug.Log("test");
                Animator.Play(anim.name);
            }
        }
    }
}
