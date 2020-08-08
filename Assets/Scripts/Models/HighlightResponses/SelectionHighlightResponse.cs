using UnityEngine;
using UnityEngine.UI;

namespace SweetAndSaltyStudios
{
    public class SelectionHighlightResponse : MonoBehaviour, IHighlightResponse
    {
        #region VARIABLES
        [Space]
        [Header("Settings")]
#pragma warning disable 0649
        [SerializeField] private float animationSpeed = 0.2f;
        [SerializeField] private float highlightScaleMultiplier = 1.025f;
        [SerializeField] private Vector2 highlightedSize;
#pragma warning restore 0649

         private Vector2 defaultSize;

        #endregion VARIABLES

        #region PROPERTIES

        public RectTransform RectTransform
        {
            get;
            private set;
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            var backgroundImage = transform.Find("BackgroundImage").GetComponent<Image>();

            if(backgroundImage == null)
            {
                return;
            }

            RectTransform = backgroundImage.rectTransform;
            defaultSize = RectTransform.localScale;
            highlightedSize = defaultSize * highlightScaleMultiplier;
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void OnHoverIn()
        {
            LeanTween.scale(RectTransform, highlightedSize, animationSpeed);
        }

        public void OnHoverOut()
        {
            LeanTween.scale(RectTransform, defaultSize, animationSpeed);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}