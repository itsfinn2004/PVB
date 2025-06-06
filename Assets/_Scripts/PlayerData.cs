using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using UnityEngine.InputSystem;
using FistFury.StateMachine;
using FistFury.StateMachine.States;
using FistFury.Entities;


//Gemaakt door finn streunding op 16 mei 2025

namespace FistFury
{

    public class PlayerData : Core
    {
        public int playernumber;
        public int health = 100;
        public int energy = 0;
        public int lifes = 3;
        public Image[] lifeimages;
        public Transform spawnpoint;
        public Slider Healthslider;
        public GameObject Healthslidergreen;
        public Slider Energyslider;
        public GameObject Energysliderblue;

        private void Awake()
        {


            if(playernumber == 1) // hier moeten de spelers spawnen
            {
            spawnpoint = GameObject.Find("SpawnPosition p1").transform;

            }
            else
            {
                spawnpoint = GameObject.Find("SpawnPosition p2").transform;

            }

        }
        private void Update()
        { //hier worden de sliders opgezet
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
        //hier zeg je waar energy erbij moet komen
        public void AddEnergy(int addenergy)
        {
            energy += addenergy;
            energy = (int)Mathf.Clamp(energy, 0, 100);
        }

    }
}


    

