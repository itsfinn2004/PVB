using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;


//Gemaakt door finn streunding op 16 mei 2025

namespace FistFury
{
    
    public class PlayerData : MonoBehaviour
    {
        
        public int health = 100;
        public int energy = 0;
        public Slider slider;
        public GameObject slidergreen;


        private void Update()
        {
            slider.value = health;
            if (health <= 1)
            {
                slidergreen.SetActive(false);
            }
            else
            {
                slidergreen.SetActive(true);
            }
        }



        public void TakeDamage(int damage)
        {
            slider.value = health;
        }

        public void GiveDamage(int damage)
        {

        }
    }
}


    

