using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SweetAndSaltyStudios
{
	public class UIButton : Button
    {
        #region VARIABLES

        private RectTransform rectTransform;
        private Vector2 defaultScale;
        private Vector2 highlightedScale;
        private float highlightScaleMultiplier = 1.05f;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        protected override void Awake()
        {
            base.Awake();

            rectTransform = GetComponent<RectTransform>();
        }

        protected override void Start()
        {
            base.Start();

            defaultScale = rectTransform.localScale;
            highlightedScale = defaultScale * highlightScaleMultiplier;
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            LeanTween.scale(rectTransform, highlightedScale, 0.1f);
        }


        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            LeanTween.scale(rectTransform, defaultScale, 0.1f);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}