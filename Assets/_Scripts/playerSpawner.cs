using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace FistFury
//Gemaakt door finn streunding op 16 mei 2025
{
    public class playerSpawner : MonoBehaviour
    {
        public Transform[] spawnPoints;
        public GameObject[] playerPrefabs;

        private int playerCount = 0;


        void Awake()
        {
            // Example setup: spawn 2 players
            SpawnPlayer("Player1Scheme", Keyboard.current);         
            SpawnPlayer("Player2Scheme", Keyboard.current);         

            if (Gamepad.all.Count >= 1)
                PlayerInput.all[0].SwitchCurrentControlScheme(Gamepad.all[0]);

            if (Gamepad.all.Count >= 2)
                PlayerInput.all[1].SwitchCurrentControlScheme(Gamepad.all[1]);
        }

        public void SpawnPlayer(string controlScheme, InputDevice device)
        {
            if (playerCount >= spawnPoints.Length) return;

            var prefab = playerPrefabs[playerCount];
            var spawnPoint = spawnPoints[playerCount].position;

            var playerInput = PlayerInput.Instantiate(
                prefab,
                playerCount,
                controlScheme,
                splitScreenIndex: -1,
                pairWithDevice: device
            );

            playerInput.transform.position = spawnPoint;
            playerCount++;
        }
    }
}
