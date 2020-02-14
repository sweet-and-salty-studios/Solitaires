using UnityEngine;
using UnityEngine.EventSystems;

namespace SweetAndSaltyStudios
{
	public class InputHandler : MonoBehaviour
    {
        #region VARIABLES

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
         
        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }

        private bool IsDoubleTap(PointerEventData eventData)
        {
            return eventData.clickCount == 2;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
         
        }

        #endregion CUSTOM_FUNCTIONS
    }
}