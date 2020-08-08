using UnityEngine;

namespace SweetAndSaltyStudios
{
    [CreateAssetMenu(fileName ="New Card Data", menuName ="Sweet & Salty Studios/Card Data")]
    public class CardData : ScriptableObject
	{
        #region VARIABLES

        public CARD_SUIT Suit;
        public bool IsRed;
        public Sprite FrontSprite;
        [HideInInspector] public Sprite BackSprite;
        public int Value;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}