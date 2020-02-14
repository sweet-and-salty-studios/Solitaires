using System;
using UnityEngine;

namespace SweetAndSaltyStudios
{
    [RequireComponent(typeof(AudioSource))]
	public class MusicPlayer : MonoBehaviour
	{
        #region VARIABLES

        public static Action<string> OnMusicChanged_Event = delegate { };

#pragma warning disable 0649
        [SerializeField] private AudioClip[] musicAudioClips;
#pragma warning restore 0649

        private MusicTrack[] musicTracks;

        private AudioSource audioSource;
        private int currentIndex;
        private bool isPaused;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            musicTracks = new MusicTrack[musicAudioClips.Length];

            for(int i = 0; i < musicTracks.Length; i++)
            {
                musicTracks[i] = new MusicTrack(musicAudioClips[i]);
            }
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void PlayMusicTrack(MusicTrack musicTrack)
        {
            if(musicTrack == null)
            {
                return;
            }

            audioSource.clip = musicTrack.AudioClip;
            audioSource.Play();

            OnMusicChanged_Event(musicTrack.Name);
        }

        public void PlayNext()
        {
            currentIndex++;

            if(currentIndex >= musicTracks.Length)
            {
                currentIndex = 0;
            }

            var nextMusicTrack = musicTracks[currentIndex];

            PlayMusicTrack(nextMusicTrack);
        }

        public void Play()
        {
            if(isPaused)
            {
                audioSource.UnPause();
            }
        }

        public void PlayPrevious()
        {
            currentIndex--;

            if(currentIndex <= 0)
            {
                currentIndex = musicTracks.Length - 1;
            }

            var previousMusicTrack = musicTracks[currentIndex];

            PlayMusicTrack(previousMusicTrack);
        }

        public void Pause()
        {
            if(audioSource.isPlaying == false)
            {
                return;
            }

            audioSource.Pause();

            isPaused = true;
        }

        #endregion CUSTOM_FUNCTIONS
    }
}