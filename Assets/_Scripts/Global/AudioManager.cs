using System.Collections.Generic;
using UnityEngine;

namespace FistFury.Global
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> audioClips;
        private Dictionary<string, AudioSource> _cachedAudioSources;
        
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
            
            InitializeAudioSources();
        }

        #endregion

        private void InitializeAudioSources()
        {
            if (audioClips.Count == 0) return;

            foreach (AudioClip clip in audioClips)
            {
                GameObject obj = new(clip.name);
                obj.transform.SetParent(transform);
                
                // configure audio source
                var source = obj.AddComponent<AudioSource>();
                source.clip = clip;
                source.playOnAwake = false;
                source.spatialBlend = 0;

                _cachedAudioSources ??= new Dictionary<string, AudioSource>();
                _cachedAudioSources.Add(clip.name, source);
            }
        }

        /// <summary>
        /// Plays an audio clip by its name.
        /// </summary>
        /// <param name="clipName">The name of the audio clip to play.</param>
        public void Play(string clipName)
        {
            _cachedAudioSources[clipName].Play();
        }
    }
}
