using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetAndSaltyStudios
{
    public abstract class EventInfo
    {

    }

    public class Test_EventInfo : EventInfo
    {
        public string Description = "This is a test event";
    }

	public class EventManager : MonoBehaviour
	{
        #region VARIABLES

        public delegate void EventListener<T>(T eventInfo) where T : EventInfo;
        private static Dictionary<Type, IList> eventListeners = new Dictionary<Type, IList>();

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public static void AddListener<T>(EventListener<T> listener) where T : EventInfo
        {
            var type = typeof(T);

            if(eventListeners.ContainsKey(type) == false)
            {
                Debug.Log($"Event Manager: Add new entry -- Add Listener type: {type.Name}");

                eventListeners.Add(type, new List<EventListener<T>>());
            }

            Debug.Log($"Event Manager -- Add Listener type: {type.Name}");
            eventListeners[type].Add(listener);

        }

        public static void Invoke<T>(T eventInfo) where T : EventInfo
        {
            var type = typeof(T);

            if(eventListeners.ContainsKey(type))
            {
                Debug.Log($"Event Manager -- Invoke event type: {type.Name}");
                foreach(EventListener<T> listener in eventListeners[type])
                {
                    listener.Invoke(eventInfo);
                }
            }
        }

        #endregion CUSTOM_FUNCTIONS
    }
}