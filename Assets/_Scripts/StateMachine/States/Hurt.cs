//Gemaakt door finn streunding op 16 mei 2025

using UnityEngine;

namespace FistFury.StateMachine.States
{
    public class Hurt : State
    {
        [SerializeField] private AnimationClip anim;
        public float iframeTimer;
        private float timer;
        private bool iframes;
        
        public override void Do()
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = 0; 
                IsComplete = true;
            }
        }
        public override void Enter()
        {
            base.Enter();
            timer = iframeTimer;
            iframes = true;
            // play animation
            if (Animator && anim)
                Animator.Play(anim.name);
           

            
        }

        public override void Exit()
        {
            base.Exit();
            iframes = false;
         
        }
    }
}
