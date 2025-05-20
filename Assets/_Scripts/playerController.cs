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
        public SpriteRenderer spriteRenderer;
        public PlayerData pd;
        public combatmanager cm;
        public bool inputEnabled = true;

        [Header("state checks")]
        private bool isDucking;
        private bool isBlocking;
        private bool isJumping;
        private bool isLpunch;
        private bool isMpunch;
        private bool isLKick;
        private bool isMKick;
        private bool isJumpKick;
        

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
        [SerializeField] private JumpKick Jumpkick;
        [SerializeField] private Special special;
        [SerializeField] private Block block;
        [SerializeField] private Hurt hurt;
        
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
            Jumpkick = GetComponentInChildren<JumpKick>();
            hurt = GetComponentInChildren<Hurt>();
            
        }

#endif

        private void Start()
        {
            cm = GetComponent<combatmanager>();
            SetupInstances();
            StateMachine.Set(idle);
        }
        private void Awake()
        {
            
            rb = GetComponent<Rigidbody2D>();
            SetupInstances();
            StateMachine.Set(idle);
        }
       


        public void onHorizontalMove(InputAction.CallbackContext context)
        {
            float input = context.ReadValue<float>();


            if (!isDucking && inputEnabled)
            {
                movement = new Vector2(input, 0f);
            }



            if (!isDucking )
            {

                if (movement.x < 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else if (movement.x > 0)
                    transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public void onJump(InputAction.CallbackContext context)
        {
            Debug.Log("spring test");

            if (context.performed && isGrounded && !isDucking && inputEnabled)
            {
                isJumping = context.ReadValueAsButton();
                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
               

            }
        }

        public void onDuck(InputAction.CallbackContext context)
        {
            if( isGrounded == true)
            {
            isDucking = context.ReadValueAsButton();
            movement = new Vector2(0f, 0f);

            }
        }
        public void onJumpKick(InputAction.CallbackContext context)
        {
            
                isJumpKick = context.ReadValueAsButton();
            
        }

        public void onLightPunch(InputAction.CallbackContext context)
        {
            
            isLpunch = context.ReadValueAsButton();
           


        }
        public void onMediumPunch(InputAction.CallbackContext context)
        {
            isMpunch = context.ReadValueAsButton();
        }
        public void onHeavyPunch(InputAction.CallbackContext context)
        {
           


        }
        public void onLightKick(InputAction.CallbackContext context)
        {
          isLKick = context.ReadValueAsButton();
        }
        public void onMediumKick(InputAction.CallbackContext context)
        {
            isMKick = context.ReadValueAsButton();
        }
        public void onSpecial(InputAction.CallbackContext context)
        {
          
        }
       
        public void onBlock(InputAction.CallbackContext context)
        {
            if (isGrounded == true)
            {
                isBlocking = context.ReadValueAsButton();
                movement = new Vector2(0f, 0f);

            }
        }



        private void OnCollisionEnter2D(Collision2D collision)
        {
            State oldState = StateMachine.CurrentState;
            State newState = idle;
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                isJumping = false;
                
            }
        }

        private void Update()
        {
            transform.Translate(movement * Time.deltaTime * 5f);
            SelectState();

        }

        private void SelectState()
        {
            State oldState = StateMachine.CurrentState;
            State newState = idle;


            if (isDucking)
                newState = duck;
            else if (isBlocking)
                newState = block;
            else if (isJumpKick && !isGrounded)
                newState = Jumpkick;
            else if (isJumping)
                newState = jump;

            else if (Mathf.Abs(movement.x) > 0.01f)
                newState = move;
            else if (isLpunch)
                newState = Lpunch;
            else if (isMpunch)
                newState = Mpunch;
            else if (isLKick)
                newState = Lkick;
            else if (isMKick)
                newState = Mkick;
            else if (cm.GotHit)
                newState = hurt;


            else
                newState = idle;

            if (newState != oldState)
                StateMachine.Set(newState);
        }
    }
}
