// Made by Finn Streunding on 16/5/2025

using UnityEngine;

namespace FistFury.StateMachine.States
{
    public class HPunch : State
    {
        [SerializeField] private AnimationClip anim;
        private float animationTimer = 0f;
        private bool animationComplete = false;
        private playerController Pc;

        private void Awake()
        {
            Pc = GetComponentInParent<playerController>();
        }

        public override void Enter()
        {
            base.Enter();


            if (Pc != null)
                Pc.inputEnabled = false;

            animationTimer = 0f;
            animationComplete = false;


            if (Animator && anim)
                Animator.Play(anim.name);
        }

        public override void Do()
        {
            base.Do();


            if (!animationComplete)
            {
                animationTimer += Time.deltaTime;

                // kijk of animatie klaar is
                if (animationTimer >= anim.length)
                {
                    animationComplete = true;
                    IsComplete = true;

                    //als animatie klaar is zorg dat je weer mag bewegen
                    if (Pc != null)
                        Pc.inputEnabled = true;
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            //zorg voor de zekerheid dat input weer aanstaat
            if (Pc != null && !Pc.inputEnabled)
                Pc.inputEnabled = true;
        }
    }
}
