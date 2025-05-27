using UnityEngine;
using UnityEngine.InputSystem;
using FistFury.StateMachine;
using FistFury.StateMachine.States;
using FistFury.Entities;

namespace FistFury
//Gemaakt door finn streunding op 16 mei 2025
//deze script zorgt ervoor dat de spelers naar de goeie states gaan na elke input en zorgt er ook voor dat de spelers kunnen bewegen
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
        Heavy,
        Special
    }

    public enum KickType
    {
        Light,
        Medium,
        JumpKick
    }


    public class playerController : Core
    {
        public Vector2 movement;
        public Rigidbody2D rb;
        public float jumpForce = 7f;
        private bool isGrounded;
        public SpriteRenderer spriteRenderer;
        public PlayerData pd;
        public combatmanager cm;
        public bool inputEnabled = true;
        public GameObject specialBall;
        public Transform specialTransform;

        [Header("state checks")]
        private bool isDucking;
        public bool isBlocking;
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
            //hier word de combat manager gemaakt en zort dat je standaard state op idle staat als je begint
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

            if (!inputEnabled || cm.beginround)
                return;

            float input = context.ReadValue<float>();

            if (!isDucking && !isAttacking && !isBlocking)
            {
                movement = new Vector2(input, 0f);
            }
            else
                movement = Vector2.zero;

            if (!isDucking && !isBlocking) // als je links of rechts gaat flipt je sprite en hitboxes
            {
                if (movement.x < 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else if (movement.x > 0)
                    transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public void onJump(InputAction.CallbackContext context)
        {
            if (!inputEnabled || cm.beginround)
                return;

            Debug.Log("spring test");

            if (context.performed && isGrounded && !isDucking)
            {
                isJumping = context.ReadValueAsButton();
                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }
        }

        public void onDuck(InputAction.CallbackContext context)
        {
            Debug.Log("Quak");
            if (!inputEnabled)
                return;

            if (isGrounded == true)
            {
                isDucking = context.ReadValueAsButton();
                movement = new Vector2(0f, 0f);
            }
        }
        public void onJumpKick(InputAction.CallbackContext context)
        {
            if (!inputEnabled)
                return;

            if (!isGrounded) // je moet in de lucht zijn voordat je dit kan doen
            {
                isAttacking = context.ReadValueAsButton();
                meleeType = MeleeType.Kick;
                kickType = KickType.JumpKick;
            }
        }

        public void onLightPunch(InputAction.CallbackContext context)
        {
            if (!inputEnabled)
                return;
            if(!isDucking)
            {

            isAttacking = context.ReadValueAsButton();
            movement = Vector2.zero;
            meleeType = MeleeType.Punch;
            punchType = PunchType.Light;
            }
        }

        public void onMediumPunch(InputAction.CallbackContext context)
        {
            if (!inputEnabled)
                return;
            if (!isDucking)
            {
                isAttacking = context.ReadValueAsButton();
                movement = Vector2.zero;
                meleeType = MeleeType.Punch;
                punchType = PunchType.Medium;
            }
        }

        public void onHeavyPunch(InputAction.CallbackContext context)
        {
            if (!inputEnabled)
                return;
            if (!isDucking)
            {
                isAttacking = context.ReadValueAsButton();
                movement = Vector2.zero;
                meleeType = MeleeType.Punch;
                punchType = PunchType.Heavy;
            }
        }

        public void onLightKick(InputAction.CallbackContext context)
        {
            if (!inputEnabled)
                return;

            if (!isDucking)
            {
                isAttacking = context.ReadValueAsButton();
                movement = Vector2.zero;
                Debug.Log("Light Kick input received");
                meleeType = MeleeType.Kick;
                kickType = KickType.Light;
            }
        }

        public void onMediumKick(InputAction.CallbackContext context)
        {
            if (!inputEnabled)
                return;
            if (!isDucking)
            {

                isAttacking = context.ReadValueAsButton();
                movement = Vector2.zero;
                Debug.Log("medium Kick input received");
                meleeType = MeleeType.Kick;
                kickType = KickType.Medium;
            }
        }

        public void onSpecial(InputAction.CallbackContext context)
        {
            if (!inputEnabled)
                return;

            if (pd.energy >= 60 && isDucking)
            {
                meleeType = MeleeType.Punch;
                punchType = PunchType.Special;
                GameObject projectile = Instantiate(specialBall, specialTransform.position, Quaternion.identity);

                // Match the scale of the shooter to determine direction
                projectile.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
                Debug.Log("HOLLOW PURPLEEEEEEEEEE");
                pd.energy -= 60;
            }
        }

        public void onBlock(InputAction.CallbackContext context)
        {
            if (!inputEnabled)
                return;


            if (isGrounded == true && !isDucking)
            {
                isBlocking = context.ReadValueAsButton();
              //  movement = new Vector2(0f, 0f);
            }
            else
                isBlocking = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground")) // ground check
            {
                isGrounded = true;
                isJumping = false;
            }
        }

        private void Update()
        {
            if (cm.beginround)
            {
                StateMachine.Set(idle, true); return;
            }
            
            Move();
            SelectState();
            CurrentState.Do();

            // Handle state completion
            if (CurrentState.IsComplete)
            { 
                if (isAttacking)
                {
                    isAttacking = false;
                }
                //hij zet de state weer op idle als isAttacking false is
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

            if (!isAttacking) //hier worden alle niet movement states ingezet
            {

                if (!isJumping && isGrounded)
                {
                    // move state logica
                    if (movement.x != 0 && isGrounded)
                        newState = move;
                    else if (isBlocking)
                        newState = block;
                    else if (isDucking)
                        newState = duck;
                    else if (cm.GotHit)
                        newState = hurt;
                    
                    else
                        newState = idle;
                }
                  
                        else // als je niet aanvalt en je bent niet grounded gaat je state op jump
                    newState = jump;
            }
            else if (isJumping && !isGrounded && isAttacking) //als je in de lucht een kick doet doe je een jmp kick
            {
                if (meleeType == MeleeType.Kick && kickType == KickType.JumpKick)
                    newState = Jumpkick;
            }
            else
            {
                if (isBlocking)
                    return;
                // logica om te weten welke punch je doet
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

                        case PunchType.Heavy:
                            newState = Hpunch;
                            break;
                        case PunchType.Special:
                            newState = special;
                            break;
                    }
                }
                //logica op welke kick je doet
                else if (meleeType == MeleeType.Kick)
                {
                    switch (kickType)
                    {
                        case KickType.Light:
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
            {
                StateMachine.Set(newState, true);
            }
        }
    }
}
