using System;
using UnityEngine;

namespace FistFury.Global
{
    /// <summary>
    /// Singleton instance of the Audio Manager, this class pools all the audio sources to itself.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        #region Singleton

        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #endregion
    }
}
