using UnityEngine;

namespace SweetAndSaltyStudios
{
    public class AudioManager : MonoBehaviour
	{
        #region VARIABLES

        [Space]
        [Header("Audio Clips")]
        public AudioClip Place_Card_Sound;
        public AudioClip Deal_Card_Sound;
        public AudioClip Turn_Card_Sound;
        public AudioClip Invalid_Card_Placement_Sound;
        public AudioClip GameStart_Sound;
        public AudioClip Shuffle_Sound;

        private AudioSource audioSource;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            RegisterSoundEvents();
        }

        private void OnDisable()
        {
            UnregisterSoundEvents();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void RegisterSoundEvents()
        {
            Stock_Pile.OnCardDrawed_Event += PlayCardFlip;

            Foundation_Pile.OnCardsInvalidPlacement_Event += PlayInvalidPlacement;
            Foundation_Pile.OnCardValidPlacement_Event += PlayValidPlacement;
            Foundation_Pile.OnQuickPlacement += PlayQuickPlacement;

            Tableau_Pile.OnCardsInvalidPlacement_Event += PlayInvalidPlacement;
            Tableau_Pile.OnCardsValidPlacement_Event += PlayValidPlacement;

            CardHolder_Pile.OnCardReset_Event += PlayCardReset;
            CardDisplay.OnFlip_Event += PlayCardFlip;
        }

        private void UnregisterSoundEvents()
        {
            Stock_Pile.OnCardDrawed_Event -= PlayCardFlip;

            Foundation_Pile.OnCardsInvalidPlacement_Event -= PlayInvalidPlacement;
            Foundation_Pile.OnCardValidPlacement_Event -= PlayValidPlacement;
            Foundation_Pile.OnQuickPlacement -= PlayQuickPlacement;

            Tableau_Pile.OnCardsInvalidPlacement_Event -= PlayInvalidPlacement;
            Tableau_Pile.OnCardsValidPlacement_Event -= PlayValidPlacement;

            CardHolder_Pile.OnCardReset_Event -= PlayCardReset;
            CardDisplay.OnFlip_Event -= PlayCardFlip;
        }

        private void PlayCardReset()
        {
            PlaySoundEffect(Invalid_Card_Placement_Sound);
        }

        private void PlayInvalidPlacement(CardDisplay[] someCards)
        {
            PlaySoundEffect(Invalid_Card_Placement_Sound);
        }

        private void PlayValidPlacement(CardDisplay[] someCards, UndoAction someUndoAction)
        {
            PlaySoundEffect(Place_Card_Sound);
        }

        private void PlayCardFlip(CardDisplay someCard, UndoAction someUndoAction)
        {
            PlaySoundEffect(Turn_Card_Sound);
        }

        private void PlayQuickPlacement(CardDisplay someCard, UndoAction someUndoAction)
        {
            PlaySoundEffect(Place_Card_Sound);
        }

        public void PlaySoundEffect(AudioClip audioClip)
        {
            if(audioSource.isPlaying)
            {
                return;
            }

            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        #endregion CUSTOM_FUNCTIONS
    }
}