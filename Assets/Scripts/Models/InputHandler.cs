using UnityEngine;
using UnityEngine.EventSystems;

namespace SweetAndSaltyStudios
{
	public class InputHandler :
    MonoBehaviour,

    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,

    IPointerDownHandler,
    IPointerClickHandler,
    IPointerUpHandler
    {
        #region VARIABLES

        private IDraggable draggable;
        private IClickable interactable;

        private float lastClick;
        private readonly float interval = 0.2f;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            draggable = GetComponentInChildren<IDraggable>();
            interactable = GetComponentInChildren<IClickable>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(draggable == null) { return; }

            draggable.OnBeginDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(draggable == null) { return; }

            draggable.OnDrag(eventData.delta);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(draggable == null) { return; }

            draggable.OnEndDrag();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(interactable == null) { return; }

            interactable.OnPointerDown();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(interactable == null) { return; }

#if UNITY_EDITOR
            if(IsDoubleTap(eventData))
            {
                interactable.OnDoubleClick();
                return;
            }
#else
            if(IsDoubleTap())
            {
                interactable.OnDoubleClick();
                return;
            }
#endif
            interactable.OnPointerClick();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(interactable == null) { return; }

            interactable.OnPointerUp();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private bool IsDoubleTap(PointerEventData eventData)
        {
            return eventData.clickCount == 2;
        }

        private bool IsDoubleTap()
        {
            if((lastClick + interval) > Time.time)
            {
                return true;
            }

            lastClick = Time.time;

            return false;
        }

#endregion CUSTOM_FUNCTIONS
    }
}