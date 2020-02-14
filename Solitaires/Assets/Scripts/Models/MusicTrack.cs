using UnityEngine;

namespace SweetAndSaltyStudios
{
    public class MusicTrack
    {
        public string Name
        {
            get;
            private set;
        }

        public AudioClip AudioClip
        {
            get;
            private set;
        }

        public MusicTrack(AudioClip audioClip)
        {
            AudioClip = audioClip;

            var originalName = audioClip.name;
            originalName = originalName.Replace("_", "");
            Name = originalName.Replace("Loop", "");
        }
    }
}