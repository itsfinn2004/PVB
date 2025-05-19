using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FistFury
{
    public class Hitbox : MonoBehaviour
    {
        public int damage = 10;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Hurtbox"))
            {
                combatmanager targetCombat = other.GetComponentInParent<combatmanager>();
                if (targetCombat != null)
                {
                    targetCombat.ReceiveHit(damage);
                }
            }
        }
    }
}
