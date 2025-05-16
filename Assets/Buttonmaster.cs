using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace FistFury
{
    public class Buttonmaster : MonoBehaviour
    {

        public GameObject mainMenu;
        public GameObject settingsMenu;
        public GameObject settingsFirstButton;
        public GameObject mapSelectFirstButton;
        public GameObject mapselect;
        public GameObject mainmenuFirstSelectButton;

        public void OpenSettings()
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);

           
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(settingsFirstButton);
        }
        public void OpenMapSelect()
        {
            mainMenu.SetActive(false);
            mapselect.SetActive(true);


            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(mapSelectFirstButton);
        }

        public void BackToMainMenu()
        {
            settingsMenu.SetActive(false);
            mapselect.SetActive(false);
            mainMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(mainmenuFirstSelectButton);


        }
        public void goToMap(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void Exit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}
