using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Gemaakt door finn streunding op 19 mei 2025

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
            playerData.energy += 8;

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
                {
                    Debug.Log($"{gameObject.name} is KO'd.");
                    playerData.lifeimages[playerData.lifes - 1].gameObject.SetActive(false);
                    playerData.lifes -= 1;

                    FindObjectOfType<RoundsManager>().NewRound(); 
                }
            }

        }

        public void onNewRound()
        {
            playerData.health = 100;
            transform.position = playerData.spawnpoint.position;
        }
    }
}
