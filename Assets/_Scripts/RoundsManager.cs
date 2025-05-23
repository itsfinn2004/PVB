using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Gemaakt door finn streunding op 19 mei 2025

namespace FistFury
{
    public class RoundsManager : MonoBehaviour
    {
        public combatmanager player1;
        public combatmanager player2;
        public playerController PlayercontrollerP1;
        public playerController PlayercontrollerP2;



        [Header("UI")]
        public TextMeshProUGUI toptimer;
        public TextMeshProUGUI middletimer;
        public TextMeshProUGUI winnerText;
        public GameObject winscreen;

        [Header("Timers")]
        public int startTime = 99;
        private float currentTime;
        private bool isRunning = false;



        private void Start()
        {
            player1 = GameObject.Find("_player_(Clone)").GetComponent<combatmanager>();
            player2 = GameObject.Find("_player2_ (Clone)").GetComponent<combatmanager>();
            PlayercontrollerP1 = GameObject.Find("_player_(Clone)").GetComponent<playerController>();
            PlayercontrollerP2 = GameObject.Find("_player2_ (Clone)").GetComponent<playerController>();
            currentTime = startTime;
            StartCoroutine(RoundStartCountdown());

        }

        private void Update()
        {
            

            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                isRunning = false;
                TimerFinished();
            }

            int seconds = Mathf.FloorToInt(currentTime);
            toptimer.text = seconds.ToString("00");
            
        }

        IEnumerator RoundStartCountdown()
        {
            
            PlayercontrollerP1.inputEnabled = false;
            PlayercontrollerP2.inputEnabled = false;

            toptimer.gameObject.SetActive(false);
            middletimer.gameObject.SetActive(true);

            middletimer.text = "3";
            yield return new WaitForSeconds(1f);

            middletimer.text = "2";
            yield return new WaitForSeconds(1f);

            middletimer.text = "1";
            yield return new WaitForSeconds(1f);

            middletimer.text = "GO!";
            yield return new WaitForSeconds(1f);

            PlayercontrollerP1.cm.beginround = false;
            PlayercontrollerP2.cm.beginround = false;
            middletimer.gameObject.SetActive(false);
            toptimer.gameObject.SetActive(true);
            StartRound(); 
        }

        public void StartRound()
        {
            currentTime = startTime;
            PlayercontrollerP1.inputEnabled = true;
            PlayercontrollerP2.inputEnabled = true;
            
        }


        public void TimerFinished()
        {
            NewRound();
        }
        public void NewRound()
        {
           
            if (player1.playerData.lifes <= 0)
            {
                Debug.Log($"Player 2  WINS!");
                winnerText.SetText("Player 2  WINS!");
                EndGame(player2);
                return;
            }
            else if (player2.playerData.lifes <= 0)
            {
                winnerText.SetText("Player 1  WINS!");
                EndGame(player1);
                return;
            }
            else
            {

            player1.onNewRound();
            player2.onNewRound();
            StartCoroutine(RoundStartCountdown());
            }
        }
        private void EndGame(combatmanager winner)
        {
            Destroy(player1.gameObject);
            Destroy(player2.gameObject);
            winscreen.SetActive(true);
            



        }

    }
}
