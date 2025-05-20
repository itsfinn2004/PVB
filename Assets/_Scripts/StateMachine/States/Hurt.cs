//Gemaakt door finn streunding op 16 mei 2025

using UnityEngine;

namespace FistFury.StateMachine.States
{
    public class Hurt : State
    {
        [SerializeField] private AnimationClip anim;
        combatmanager cm;

        private void Awake()
        {
            cm = GetComponentInParent<combatmanager>();
        }

        public override void Enter()
        {
            base.Enter();

            // play animation
            if (Animator && anim)
                Animator.Play(anim.name);
        }
    }
}
