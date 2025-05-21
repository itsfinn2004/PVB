using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//gemaakt door finn op 21 mei 2025
namespace FistFury
{
    public class specialBall : MonoBehaviour
    {
        public float shootForce = 10f;

        void Start()
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
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
    }
}
