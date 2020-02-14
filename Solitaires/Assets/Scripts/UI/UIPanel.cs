using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SweetAndSaltyStudios
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : MonoBehaviour, IInteractable, IDraggable
    {
        #region VARIABLES

        private RectTransform rectTransform;
        protected CanvasGroup canvasGroup;

        #endregion VARIABLES

        #region PROPERTIES

        protected RectTransform RectTransform
        {
            get
            {
                if(rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }

                return rectTransform;
            }
        }

        protected CanvasGroup CanvasGroup
        {
            get
            {
                if(canvasGroup == null)
                {
                    canvasGroup = GetComponent<CanvasGroup>();
                }

                return canvasGroup;
            }
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {

        }

        public void OnCancel(BaseEventData eventData)
        {

        }

        public void OnDeselect(BaseEventData eventData)
        {

        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            var position = RectTransform.anchoredPosition;

            RectTransform.anchoredPosition += eventData.delta;

            if(IsRectTransformInsideSreen(RectTransform) == false)
            {
                RectTransform.anchoredPosition = position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {

        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        protected virtual int StartOpenAnimation(float animationDuration)
        {
            RectTransform.localScale = Vector2.zero;

            return LeanTween.scale(RectTransform, Vector3.one, animationDuration)
                .setOnStart(() =>
                {
                    CanvasGroup.blocksRaycasts = false;
                })
                .setEaseOutCubic()
                .setOnComplete(() =>
                {
                    CanvasGroup.blocksRaycasts = true;
                }).id;
        }

        protected virtual int StartCloseAnimation(float aniamtionDuration)
        {
            return LeanTween.scale(RectTransform, new Vector3(0, 0, 1), aniamtionDuration)
             .setOnStart(() =>
             {
                 CanvasGroup.blocksRaycasts = false;
             }).id;
        }

        public IEnumerator Open()
        {
            var animationID = StartOpenAnimation(0.25f);

            yield return new WaitUntil(() => LeanTween.isTweening(animationID));
        }

        public IEnumerator Close()
        {
            canvasGroup.blocksRaycasts = false;

            var animationID = StartCloseAnimation(0.25f);

            yield return new WaitUntil(() => LeanTween.isTweening(animationID));
        }

        private bool IsRectTransformInsideSreen(RectTransform rectTransform)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            var visibleCorners = 0;
            var rect = new Rect(0, 0, Screen.width, Screen.height);

            for(int i = 0; i < corners.Length; i++)
            {
                if(rect.Contains(corners[i]))
                {
                    visibleCorners++;
                }
            }

            return visibleCorners == 4;
        }

        #endregion CUSTOM_FUNCTIONS
    }
}