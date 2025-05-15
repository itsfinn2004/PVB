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
            foreach (var device in Gamepad.all)
            {
                SpawnPlayer(device);
            }
        }

        public void SpawnPlayer(InputDevice device)
        {
            if (playerCount >= spawnPoints.Length)
            {
                Debug.LogWarning("Too many players!");
                return;
            }

            var prefab = playerPrefabs[playerCount];
            var spawnPoint = spawnPoints[playerCount].position;

            PlayerInput playerInput = PlayerInput.Instantiate(prefab, playerCount, null, -1 , device);

            playerInput.transform.position = spawnPoint;

            playerCount++;
        }
    }
}
