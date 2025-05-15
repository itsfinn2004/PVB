using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace FistFury
    // deze script is gemaakt door finn streunding
{
    public class playerSpawner : MonoBehaviour
    {
        public Transform[] spawnPoints;
        private int playerCount = 0;

        public void OnPlayerJoined(PlayerInput playerInput)
        {
            if (playerCount < spawnPoints.Length)
            {
                playerInput.transform.position = spawnPoints[playerCount].position;
            }

            playerCount++;
        }
    }
}
