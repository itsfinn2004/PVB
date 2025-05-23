using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//gemaakt door finn op 21 mei 2025
namespace FistFury
{
    public class specialBall : MonoBehaviour
    {
        public float shootForce = 10f;
        public playerController PlayercontrollerP1;
        public playerController PlayercontrollerP2;

        void Start()
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            PlayercontrollerP1 = GameObject.Find("_player_(Clone)").GetComponent<playerController>();
            PlayercontrollerP2 = GameObject.Find("_player2_ (Clone)").GetComponent<playerController>();

         
            
            if (rb != null)
            {
                // Check the local scale to determine direction (1 = right, -1 = left)
                float direction = transform.localScale.x > 0 ? 1f : -1f;

                // Move the projectile in that direction
                rb.velocity = new Vector2(direction * shootForce, 0f);
            }
            else
            {
                Debug.LogWarning("No Rigidbody2D found on the projectile.");
            }
        }
        private void Update()
        {
            if (PlayercontrollerP1.cm.beginround || PlayercontrollerP2.cm.beginround)
            {
                Destroy(gameObject);
                
            }
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}
