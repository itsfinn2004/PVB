using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FistFury
{
    public class combatmanager : MonoBehaviour
    {
        public PlayerData playerData;

        private void Awake()
        {
            if (playerData == null)
                playerData = GetComponent<PlayerData>();
        }

        public void ReceiveHit(int damage)
        {
            Debug.Log($"{gameObject.name} received {damage} damage!");

            playerData.health -= damage;

            if (playerData.health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log($"{gameObject.name} is KO'd.");
            // Handle death logic
        }
    }
}
