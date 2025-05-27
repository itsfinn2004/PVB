using System;
using UnityEngine;

//Gemaakt door finn streunding op 19 mei 2025

namespace FistFury
{
    public class combatmanager : MonoBehaviour
    {
        //deze script zorgt voor de combat en wat er gebeurd als je gehit word
        public PlayerData playerData;
        public playerController pc;
        public bool GotHit;
        public bool beginround;

        // dit is hoelang je in de hurt state blijft
        [SerializeField] private float hurtStateTime = 1.0f;
        private Coroutine hurtCoroutine;



        private void Awake()
        {
            
                playerData = GetComponent<PlayerData>();
            pc = GetComponent<playerController>();
        }

        public void ReceiveHit(int damage)//dit hebeurt er als je gehit word
        {
            Debug.Log($"{gameObject.name} received {damage} damage!");
            if(pc.isBlocking)
            {
                playerData.health -= damage/10;
            }
            else
            {
            playerData.health -= damage;
            }
            playerData.energy += 8;

            GotHit = true;

            if (playerData.health <= 0)
            {
                Die();
            }
        }
       

        private void Die()
        {
            if (playerData.lifes == 0)
            {
                // spel klaar
            }
            else
            {
                Debug.Log($"{gameObject.name} is KO'd.");
                playerData.lifeimages[playerData.lifes - 1].gameObject.SetActive(false);
                playerData.lifes -= 1;

                FindObjectOfType<RoundsManager>().NewRound();
            }
        }

        public void onNewRound()
        {
           
            beginround = true;
            playerData.health = 100;
            transform.position = playerData.spawnpoint.position;
            pc.inputEnabled = false;

        }
    }
}