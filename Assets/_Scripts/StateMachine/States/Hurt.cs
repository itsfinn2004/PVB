//Gemaakt door finn streunding op 16 mei 2025

using FistFury.Global;
using UnityEngine;

namespace FistFury.StateMachine.States
{
    public class Hurt : State
    {
        [SerializeField] private AnimationClip anim;
        [SerializeField] private combatmanager cm;
        public float iframeTimer;
        private float timer;

        
        public override void Enter()
        {
            base.Enter();
            timer = iframeTimer;
            
            AudioManager.Instance.Play("hurt");
            
            // play animation
            if (Animator && anim)
                Animator.Play(anim.name);
        }

        public override void Do()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                //Debug.Log("I FINISHED");
                IsComplete = true;
            }
        }

        public override void Exit()
        {
            base.Exit();
            //print("ahhahahahahahahahahAHAIDJAIOJDAOIJDOI");
            cm.GotHit = false;
        }
    }
}
