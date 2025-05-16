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
        public GameObject[] playerPrefabs; 

        private int playerCount = 0;

        void Start()
        {
            SpawnPlayer("Player1Keyboard");
            SpawnPlayer("Player2Keyboard");
        }

        public void SpawnPlayer(string controlScheme)
        {
            if (playerCount >= spawnPoints.Length)
            {
                Debug.LogWarning("Too many players!");
                return;
            }

            var prefab = playerPrefabs[playerCount];
            var spawnPoint = spawnPoints[playerCount].position;

            var playerInput = PlayerInput.Instantiate(
                prefab,
                playerCount,
                controlScheme: controlScheme,
                splitScreenIndex: -1,
                pairWithDevice: null
            );

            playerInput.transform.position = spawnPoint;
            playerCount++;
        }
    }
}
