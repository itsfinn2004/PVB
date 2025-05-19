using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FistFury
{
    public class RoundsManager : MonoBehaviour
    {
        public combatmanager player1;
        public combatmanager player2;

        private void Start()
        {
            player1 = GameObject.Find("_player_(Clone)").GetComponent<combatmanager>();
            player2 = GameObject.Find("_player2_ (Clone)").GetComponent<combatmanager>();
        }
        public void NewRound()
        {
            // Check if either player is out of lives
            if (player1.playerData.lifes <= 0)
            {
                Debug.Log($"{player2.gameObject.name} WINS!");
                EndGame(player2);
                return;
            }
            else if (player2.playerData.lifes <= 0)
            {
                Debug.Log($"{player1.gameObject.name} WINS!");
                EndGame(player1);
                return;
            }

            player1.onNewRound();
            player2.onNewRound();
        }
        private void EndGame(combatmanager winner)
        {
        //zet hier de code neer die je moet hebben als de game klaar is  
        

            
        }

    }
}
