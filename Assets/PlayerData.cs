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
        public int lifes = 3;
        public Image[] lifeimages;
        public Slider Healthslider;
        public GameObject Healthslidergreen;
        public Slider Energyslider;
        public GameObject Energysliderblue;


        private void Update()
        {
            Healthslider.value = health;
            Energyslider.value = energy;
            if (health <= 1)
            {
                Healthslidergreen.SetActive(false);
            }
            else
            {
                Healthslidergreen.SetActive(true);
            }
            if (energy <= 1)
            {
                Energysliderblue.SetActive(false);
            }
            else
            {
                Energysliderblue.SetActive(true);
            }

        }

        public void AddEnergy(int addenergy)
        {
            energy += addenergy;
            energy = (int)Mathf.Clamp(energy, 0, 100);
        }

    }
}


    

