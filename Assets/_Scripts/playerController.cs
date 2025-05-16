using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FistFury.StateMachine;
using FistFury.StateMachine.States;
using FistFury.Entities;

namespace FistFury
//Gemaakt door finn streunding op 16 mei 2025
{

    public class playerController : Core
    {
        private Vector2 movement;
        public Rigidbody2D rb;
        public float jumpForce = 7f;
        private bool isGrounded;

        [Header("Behaviors")]
        [SerializeField] private Idle idle;

        [Space()]
        [SerializeField] private Move move;
        [SerializeField] private Jump jump;
        [SerializeField] private Duck duck;
        [SerializeField] private LPunch Lpunch;
        [SerializeField] private MPunch Mpunch;
        [SerializeField] private HPunch Hpunch;
        [SerializeField] private LKick Lkick;
        [SerializeField] private MKick Mkick;
        [SerializeField] private Special special;
        [SerializeField] private Block block;
#if UNITY_EDITOR

        private void OnValidate()
        {
            idle = GetComponentInChildren<Idle>();
            move = GetComponentInChildren<Move>();
            jump = GetComponentInChildren<Jump>();
            duck = GetComponentInChildren<Duck>();
            Lpunch = GetComponentInChildren<LPunch>();
            Mpunch = GetComponentInChildren<MPunch>();
            Hpunch = GetComponentInChildren<HPunch>();
            Lkick = GetComponentInChildren<LKick>();
            Mkick = GetComponentInChildren<MKick>();
            special = GetComponentInChildren<Special>();
            block = GetComponentInChildren<Block>();
        }

#endif

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            SetupInstances();
            StateMachine.Set(idle);
        }

        public void onHorizontalMove(InputAction.CallbackContext context)
        {

            State oldState = StateMachine.CurrentState;
            State newState;


          
            newState = move;
            Debug.Log(newState);
            float input = context.ReadValue<float>();
            movement = new Vector2(input, 0f);
            newState = idle;
            Debug.Log(newState);

         
             if (context.canceled)
            {
               newState = idle;
                Debug.Log("de state is nu idle als het goed is... " + newState);
            }


        }

        public void onJump(InputAction.CallbackContext context)
        {
            Debug.Log("spring test");
            if (context.performed && isGrounded)
            {
                State oldState = StateMachine.CurrentState;
                State newState;
                newState = jump;
                Debug.Log(newState);
                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
               

            }
        }

        public void onDuck(InputAction.CallbackContext context)
        {
            State oldState = StateMachine.CurrentState;
            State newState;
            newState = duck;
            Debug.Log(newState);
        }

        public void onLightPunch(InputAction.CallbackContext context)
        {
            State oldState = StateMachine.CurrentState;
            State newState;
            newState = Lpunch;
            Debug.Log(newState);
        }
        public void onMediumPunch(InputAction.CallbackContext context)
        {
            State oldState = StateMachine.CurrentState;
            State newState;
            newState = Mpunch;
            Debug.Log(newState);
        }
        public void onHeavyPunch(InputAction.CallbackContext context)
        {
            State oldState = StateMachine.CurrentState;
            State newState;
            newState = Hpunch;
            Debug.Log(newState);
        }
        public void onLightKick(InputAction.CallbackContext context)
        {
            State oldState = StateMachine.CurrentState;
            State newState;
            newState = Lkick;
            Debug.Log(newState);
        }
        public void onMediumKick(InputAction.CallbackContext context)
        {
            State oldState = StateMachine.CurrentState;
            State newState;
            newState = Mkick;
            Debug.Log(newState);
        }
        public void onSpecial(InputAction.CallbackContext context)
        {
            State oldState = StateMachine.CurrentState;
            State newState;
            newState = special;
            Debug.Log(newState);
        }
        public void onBlock(InputAction.CallbackContext context)
        {
            State oldState = StateMachine.CurrentState;
            State newState;
            newState = block;
            Debug.Log(newState);
        }



        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                State oldState = StateMachine.CurrentState;
                State newState;
                newState = idle;
                Debug.Log("de state is nu idle als het goed is... " + newState);
            }
        }

        private void Update()
        {
            transform.Translate(movement * Time.deltaTime * 5f);
        }
    }
}
