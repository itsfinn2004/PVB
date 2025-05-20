using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Gemaakt door finn streunding op 16 mei 2025
namespace FistFury
{
    public class Hitbox : MonoBehaviour
    {
        PlayerData pd;
        public int damage = 10;
        private void Awake()
        {
            pd = GetComponentInParent<PlayerData>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Hurtbox"))
            {
                combatmanager targetCombat = other.GetComponentInParent<combatmanager>();
                if (targetCombat != null)
                {
                    targetCombat.ReceiveHit(damage);
                    pd.AddEnergy(4);
                }
            }
        }
    }
}
