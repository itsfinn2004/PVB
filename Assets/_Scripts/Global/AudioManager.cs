// Made by Niek Melet at 20/5/2025

using System.Collections.Generic;
using UnityEngine;

namespace FistFury.Global
{
    /// <summary>
    /// Manages audio playback and caching of audio sources within the game.
    /// Provides functionality to play audio clips by name and ensures a centralized audio management system.
    /// </summary>
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

        /// <summary>
        /// Initializes audio sources for all audio clips in the <see cref="audioClips"/> list.
        /// </summary>
        /// <remarks>
        /// This method creates a new <see cref="GameObject"/> for each audio clip, attaches an <see cref="AudioSource"/> 
        /// component to it, and configures the audio source. The initialized audio sources are cached in a dictionary 
        /// for efficient access.
        /// </remarks>
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
