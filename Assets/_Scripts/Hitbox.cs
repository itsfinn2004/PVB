using UnityEngine;
//Gemaakt door finn streunding op 16 mei 2025
namespace FistFury
{
    public class Hitbox : MonoBehaviour
    {
        // deze script zet je op alle hitboxen om te zorgen dat je damage doet
        combatmanager cm;
        PlayerData pd;
        public int damage = 10;
        private void Start()
        {
            pd = GetComponentInParent<PlayerData>();
            cm = GetComponentInParent<combatmanager>();
           pd = GetComponentInParent<playerController>().pd;
        }
        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Hurtbox"))
            {
                combatmanager targetCombat = other.GetComponentInParent<combatmanager>();
                if (targetCombat != null && !targetCombat.GotHit)// als je iemand hit met deze script op de hitbox gebeurt dit
                {
                    targetCombat.ReceiveHit(damage);
                    pd.AddEnergy(4);
                }
            }
        }
    }
}
