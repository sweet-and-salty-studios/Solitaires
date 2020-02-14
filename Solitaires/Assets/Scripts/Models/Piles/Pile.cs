using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SweetAndSaltyStudios
{
    public abstract class Pile : MonoBehaviour
    {
        #region VARIABLES    

        public static Action OnCardPlaced_Event = delegate { };

        protected List<Card> cardsInContainer = new List<Card>();
        protected Transform content;
        protected Image backgroundImage;
        protected IHighlightResponse highlightResponse;

        private TextMeshProUGUI cardDisplayCounter;

        #endregion VARIABLES

        #region PROPERTIES

        public RectTransform RectTransform
        {
            get;
            private set;
        }
        public Transform Content
        {
            get
            {
                return content;
            }

        }
        public int CardCount
        {
            get
            {
                return cardsInContainer.Count;
            }
        }     

        protected TextMeshProUGUI CardDisplayCounter
        {
            get
            {
                if(cardDisplayCounter == null)
                {
                    cardDisplayCounter = GetComponentInChildren<TextMeshProUGUI>(true);
                }

                return cardDisplayCounter;
            }
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        protected virtual void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            backgroundImage = transform.Find("BackgroundImage").GetComponent<Image>();
            content = transform.Find("Content");
            highlightResponse = GetComponentInChildren<IHighlightResponse>();

            RegisterEvents();
        }

        protected virtual void Start() { }

        protected void OnDestroy()
        {
            UnregisterEvents();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        protected virtual void RegisterEvents() { }

        protected virtual void UnregisterEvents() { }

        public void HandleTopCard(bool interactability)
        {
            if(cardsInContainer == null)
            {
                return;
            }

            if(cardsInContainer.Count == 0)
            {
                return;
            }

            var topCard = cardsInContainer[cardsInContainer.Count - 1];

            if(topCard.IsTurned)
            {
                return;
            }

            topCard.SetInteractability(interactability);
        }

        public void HandlePreviousCard(bool interactability)
        {
            if(cardsInContainer == null)
            {
                return;
            }

            if(cardsInContainer.Count < 2)
            {
                return;
            }

            var previousCard = cardsInContainer[cardsInContainer.Count - 2];

            if(previousCard.IsTurned)
            {
                return;
            }

            previousCard.SetInteractability(interactability);
        }

        public Card[] GetAllCards()
        {
            var result = new Card[cardsInContainer.Count ];

            for(int i = 0; i < result.Length; i++)
            {
                result[i] = GetTopCard();
            }

            return result;
        }

        public Card[] GetValidCards(Card startCard)
        {
            var hitIndex = 0;

            var list = new List<Card>();

            for(; hitIndex < cardsInContainer.Count; hitIndex++)
            {
                if(cardsInContainer[hitIndex] == startCard)
                {
                    list.Add(cardsInContainer[hitIndex]);
                    break;
                }
            }

            hitIndex++;

            for(; hitIndex < cardsInContainer.Count; hitIndex++)
            {
                list.Add(cardsInContainer[hitIndex]);
            }

            var result = new Card[list.Count];

            for(int i = 0; i < list.Count; i++)
            {
                result[i] = GetCard(list[i]);
            }

            return result;
        }

        public Card GetTopCard()
        {
            if(cardsInContainer == null || cardsInContainer.Count == 0)
            {
                Debug.LogWarning("Conainer was NULL or 0");
                return null;
            }

            return GetCard(cardsInContainer[CardCount - 1]);
        }

        public Card GetCard(Card card)
        {
            if(cardsInContainer == null || card == null)
            {
                Debug.LogWarning("Conainer or card was NULL");
                return null;
            }

            for(int i = 0; i < cardsInContainer.Count; i++)
            {
                if(cardsInContainer[i] == card)
                {
                    var cardToGet = cardsInContainer[i];

                    cardsInContainer.Remove(cardToGet);

                    CardDisplayCounter.text = $"{CardCount}";

                    return cardToGet;
                }
            }

            return null;
        }

        public void PlaceCards(Card[] cards)
        {
            if(cards == null)
            {
                Debug.LogWarning("cards was NULL");
                return;
            }

            if(cards.Length == 0)
            {
                Debug.LogWarning("cards lenght was 0");
                return;
            }

            for(int i = cards.Length - 1; i >= 0; i--)
            {
                PlaceCard(cards[i]);
            }
        }

        public void PlaceCard(Card card)
        {
            if(cardsInContainer == null || card == null)
            {
                Debug.LogWarning("Conainer or card was NULL");
                return;
            }

            if(cardsInContainer.Contains(card))
            {
                Debug.LogWarning($"{name} conainer already contains card: {card.name}");
                return;
            }

            cardsInContainer.Add(card);

            card.SetPileAndParent(this);

            CardDisplayCounter.text = $"{CardCount}";

            OnCardPlaced_Event();
        }

        public bool IsCardLastIndex(Card card)
        {
            return cardsInContainer[cardsInContainer.Count - 1] == card;
        }

        #endregion CUSTOM_FUNCTIONS
    }
}