using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FistFury.StateMachine;
using FistFury.StateMachine.States;
using FistFury.Entities;

namespace FistFury
    //deze script is gemaakt door Finn Streunding
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
#if UNITY_EDITOR

        private void OnValidate()
        {
            idle = GetComponentInChildren<Idle>();
            move = GetComponentInChildren<Move>();
            jump = GetComponentInChildren<Jump>();
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


        }

        public void onJump(InputAction.CallbackContext context)
        {
            Debug.Log("spring test");
            if (context.performed && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;

            }
        }

        public void onDuck(InputAction.CallbackContext context)
        {
            
        }

        public void onAttack(InputAction.CallbackContext context)
        {

        }



        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }

        private void Update()
        {
            transform.Translate(movement * Time.deltaTime * 5f);
        }
    }
}
