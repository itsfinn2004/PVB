using UnityEngine;
//Gemaakt door finn streunding op 16 mei 2025
namespace FistFury
{
    public class Hitbox : MonoBehaviour
    {
        combatmanager cm;
        PlayerData pd;
        public int damage = 10;
        private void Awake()
        {
            pd = GetComponentInParent<PlayerData>();
            cm = GetComponentInParent<combatmanager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Hurtbox"))
            {
                combatmanager targetCombat = other.GetComponentInParent<combatmanager>();
                if (targetCombat != null && !targetCombat.GotHit)
                {
                    targetCombat.ReceiveHit(damage);
                    pd.AddEnergy(4);
                }
            }
        }
    }
}
