using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FistFury
    //deze script is gemaakt door Finn Streunding
{
    public class playerController : MonoBehaviour
    {
        private Vector2 movement;
        public Rigidbody2D rb;
        public float jumpForce = 7f;
        private bool isGrounded;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void onHorizontalMove(InputAction.CallbackContext context)
        {
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
