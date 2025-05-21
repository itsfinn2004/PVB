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
                
                rb.velocity = transform.right * shootForce;
            }
            else
            {
                Debug.LogWarning("Geen Rigidbody2D gevonden op het object.");
            }
            
        }
       
    }
}
