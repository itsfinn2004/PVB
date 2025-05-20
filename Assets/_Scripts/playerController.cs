using UnityEngine;
using UnityEngine.InputSystem;
using FistFury.StateMachine;
using FistFury.StateMachine.States;
using FistFury.Entities;

namespace FistFury
//Gemaakt door finn streunding op 16 mei 2025
{
    public enum MeleeType
    {
        Punch,
        Kick
    }

    public enum PunchType
    {
        Light,
        Medium,
        Heavy
    }

    public enum KickType
    {
        Light,
        Medium,
        JumpKick
    }


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

        // attack state checks
        private bool isAttacking;
        private MeleeType meleeType;
        private PunchType punchType;
        private KickType kickType;
        

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


            if (!isDucking && inputEnabled && !isAttacking)
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
            
            isAttacking = context.ReadValueAsButton();
            meleeType = MeleeType.Kick;
            kickType = KickType.JumpKick;
            
        }

        public void onLightPunch(InputAction.CallbackContext context)
        {
            
            //isPunching = context.ReadValueAsButton();
            isAttacking = context.ReadValueAsButton();

            movement = Vector2.zero;
            meleeType = MeleeType.Punch;
            punchType = PunchType.Light; 

        }
        public void onMediumPunch(InputAction.CallbackContext context)
        {
            isAttacking = context.ReadValueAsButton();

            movement = Vector2.zero;
            meleeType = MeleeType.Punch;
            punchType = PunchType.Medium;
        }
        public void onHeavyPunch(InputAction.CallbackContext context)
        {
            isAttacking = context.ReadValueAsButton();

            movement = Vector2.zero;
            meleeType = MeleeType.Punch;
            punchType = PunchType.Heavy;

        }
        public void onLightKick(InputAction.CallbackContext context)
        {
          //isLKick = context.ReadValueAsButton();
            isAttacking = context.ReadValueAsButton();
            movement = Vector2.zero;

            meleeType = MeleeType.Kick;
            kickType = KickType.Light;
        }
        public void onMediumKick(InputAction.CallbackContext context)
        {
            isAttacking = context.ReadValueAsButton();
            movement = Vector2.zero;

            meleeType = MeleeType.Kick;
            kickType = KickType.Medium;
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
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                isJumping = false;
                
            }
        }

        private void Update()
        {
            Move();
            SelectState();
            CurrentState.Do();
            if (CurrentState.IsComplete && CurrentState is Hurt)  // haha, niet vragen. xoxo niek
            {
                StateMachine.Set(idle, true);
            }
        }

        private void Move()
        {
            if (!isAttacking)
                transform.Translate(movement * Time.deltaTime * 5f);
        }

        private void SelectState()
        {
            State oldState = StateMachine.CurrentState;
            State newState = null;
            
            if (!isAttacking)
            {
                if (!isJumping && isGrounded)
                {
                    // move state logic
                    if (movement.x != 0 && isGrounded)
                        newState = move;
                    else if (isDucking)
                        newState = duck;
                    else if (cm.GotHit)
                        newState = hurt;
                    else
                        newState = idle;
                }
                else
                    newState = jump;            
            }
            else if (isJumping && !isGrounded && isAttacking)
            {
                if (meleeType == MeleeType.Kick && kickType == KickType.JumpKick)
                    newState = Jumpkick;
            }
            else
            {
                Debug.Log($"Melee Type: {meleeType.ToString()}");
                if (meleeType == MeleeType.Punch)
                {
                    switch (punchType)
                    {
                        case PunchType.Light:
                            newState = Lpunch;
                            break;

                        case PunchType.Medium:
                            newState = Mpunch;
                            break;
                    }
                }
                else if (meleeType == MeleeType.Kick)
                {
                    Debug.Log("jackie cha");
                    switch (kickType)
                    {
                        case KickType.Light:
                            Debug.Log("robert downey jr ");
                            newState = Lkick;
                            break;

                        case KickType.Medium:
                            newState = Mkick;
                            break;

                        default:
                            Debug.Log("Dit zou niet moeten gebeuren????");
                            break;
                    }
                }
                
            }
            

            if (newState != null && newState != oldState)
                StateMachine.Set(newState, true);
        }
    }
}
