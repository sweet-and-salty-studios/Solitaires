using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SweetAndSaltyStudios
{
    public class InputModule : MonoBehaviour
    {

    }

	public class InputManager : MonoBehaviour
	{
        #region VARIABLES

        private GraphicRaycaster raycaster;
        private PointerEventData pointerEventData;
        private EventSystem eventSystem;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            raycaster = FindObjectOfType<GraphicRaycaster>();
            eventSystem = FindObjectOfType<EventSystem>();
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                pointerEventData = new PointerEventData(eventSystem)
                {
                    position = Input.mousePosition
                };

                var raycastResults = new List<RaycastResult>();

                raycaster.Raycast(pointerEventData, raycastResults);

                if(raycastResults.Count == 0)
                {
                    return;
                }

                var hittedElement = raycastResults[0];



                //foreach(RaycastResult result in raycastResults)
                //{
                //    Debug.Log("Hit " + result.gameObject.name);
                //}
            }
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}