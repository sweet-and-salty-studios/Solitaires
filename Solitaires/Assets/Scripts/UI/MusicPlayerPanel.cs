using TMPro;
using UnityEngine;

namespace SweetAndSaltyStudios
{
	public class MusicPlayerPanel : UIPanel
	{
        #region VARIABLES

        private TextMeshProUGUI currentTrackLabelText;
        private Transform content;
        private bool isHidden;

        private Vector2 defaultSize;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        protected override void Awake()
        {
            currentTrackLabelText = GetComponentInChildren<TextMeshProUGUI>();
            content = transform.Find("Content");

            defaultSize = RectTransform.sizeDelta;

            MusicPlayer.OnMusicChanged_Event += UpdateMusicTrackLabelText;

            base.Awake();
        }

        private void OnDestroy()
        {
            MusicPlayer.OnMusicChanged_Event -= UpdateMusicTrackLabelText;
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void UpdateMusicTrackLabelText(string trackName)
        {
            currentTrackLabelText.text = trackName;
        }

        public void ShowHideButton()
        {
            isHidden = !isHidden;

            if(isHidden)
            {
                LeanTween.scale(content.gameObject, Vector2.zero, 0.1f)
                .setOnComplete(() => 
                {
                    RectTransform.sizeDelta = Vector2.zero;
                });
            }
            else
            {
                LeanTween.scale(content.gameObject, Vector2.one, 0.1f) 
                .setOnComplete(() => 
                {
                    RectTransform.sizeDelta = defaultSize;
                });
            }
        }

        #endregion CUSTOM_FUNCTIONS
    }
}