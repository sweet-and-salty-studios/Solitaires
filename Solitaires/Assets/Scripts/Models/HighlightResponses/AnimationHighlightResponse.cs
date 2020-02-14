using UnityEngine;
using UnityEngine.UI;

namespace SweetAndSaltyStudios
{
    public class AnimationHighlightResponse : MonoBehaviour, IHighlightResponse
    {
        #region VARIABLES

        [Space]
        [Header("Settings")]
#pragma warning disable 0649
        [SerializeField] private float animationSpeed = 0.2f;
        [SerializeField] private float highlightScaleMultiplier = 1.025f;
        [SerializeField] private Color highlightedColor = Color.magenta;
#pragma warning restore 0649
        private Color defaultColor;

        private Vector2 defaultSize;
        private Vector2 highlightedSize;

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
            defaultColor = backgroundImage.color;
            highlightedSize = defaultSize * highlightScaleMultiplier;
        }

        #endregion UNITY_FUNCTIONS      

        #region CUSTOM_FUNCTIONS

        public void HoverInAnimation()
        {
            StartValidPlacementAniamtion();
        }

        public void HoverOutAnimation()
        {
            EndValidPlacementAnimation();
        }

        public void StartValidPlacementAniamtion()
        {
            if(RectTransform == null)
            {
                return;
            }

            if(LeanTween.isTweening(RectTransform))
            {
                LeanTween.cancelAll();
                return;
            }

            LeanTween.scale(RectTransform, highlightedSize, animationSpeed)
            .setOnStart(()=> 
            {
                LeanTween.color(RectTransform, highlightedColor, animationSpeed)
                .setLoopPingPong();
            })
            .setLoopPingPong();
        }

        public void EndValidPlacementAnimation()
        {
            if(RectTransform == null)
            {
                return;
            }

            if(LeanTween.isTweening(RectTransform) == false)
            {
                return;
            }

            LeanTween.cancel(RectTransform);

            LeanTween.scale(RectTransform, defaultSize, animationSpeed)
            .setFrom(RectTransform.localScale)
            .setOnComplete(() => 
            {
                LeanTween.color(RectTransform, defaultColor, 0);
            });
        }

        #endregion CUSTOM_FUNCTIONS
    }
}