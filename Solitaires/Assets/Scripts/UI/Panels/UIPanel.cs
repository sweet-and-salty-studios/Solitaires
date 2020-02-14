using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SweetAndSaltyStudios
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel :
    MonoBehaviour, 
    IDraggable
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
            OnBeginDrag();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            OnDrag(eventData.delta);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDrag();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {

        }

        public virtual void OnPointerExit(PointerEventData eventData)
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

        public void OnBeginDrag()
        {
            transform.SetAsLastSibling();
        }

        public void OnDrag(Vector3 deltaPosition)
        {
            var position = RectTransform.position;

            RectTransform.position += deltaPosition;

            if(IsRectTransformInsideSreen(RectTransform) == false)
            {
                RectTransform.position = position;
            }
        }

        public void OnEndDrag()
        {
        }

        #endregion CUSTOM_FUNCTIONS
    }
}