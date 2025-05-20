using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using FistFury.StateMachine;
using FistFury.StateMachine.States;
using FistFury.Entities;

//Gemaakt door finn streunding op 19 mei 2025

namespace FistFury
{
    public class combatmanager : MonoBehaviour
    {
        public PlayerData playerData;
        public bool GotHit;
        public bool CanGetHit = true;

        // Time to stay in hurt state before returning to idle
        [SerializeField] private float hurtStateTime = 1.0f;
        private Coroutine hurtCoroutine;



        private void Awake()
        {
            if (playerData == null)
                playerData = GetComponent<PlayerData>();
        }

        public void ReceiveHit(int damage)
        {
            if (CanGetHit)
            {
                Debug.Log($"{gameObject.name} received {damage} damage!");

                playerData.health -= damage;
                playerData.energy += 8;

                GotHit = true;
                CanGetHit = false;


                if (playerData.health <= 0)
                {
                    Die();
                }
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