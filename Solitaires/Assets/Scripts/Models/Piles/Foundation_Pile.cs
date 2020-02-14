using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SweetAndSaltyStudios
{
    public class Foundation_Pile : Pile, IDroppable
    {
        #region VARIABLES

        public static event Func<Foundation_Pile, bool, bool> OnFull_Event = delegate { return false; };

        public static event Func<CardDisplay[]> OnCardsDrop_Event = delegate { return null; };
        public static event Action<CardDisplay[]> OnCardsInvalidPlacement_Event = delegate { };
        public static event Action<CardDisplay[], UndoAction> OnCardValidPlacement_Event = delegate { };
        public static event Action<CardDisplay, UndoAction> OnQuickPlacement = delegate { };

        private const int EMPTY_PILE_START_VALUE = 1;
        private const int MAX_CARD_LIMIT = 13;

#pragma warning disable 0649
        [SerializeField] private CARD_SUIT suitType;
#pragma warning restore 0649

        private Image icon;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        protected override void Awake()
        {
            base.Awake();

            icon = transform.Find("Icon").GetComponent<Image>();      
        }

        protected override void RegisterEvents()
        {
            CardDisplay.OnBeginDrag_Event += VisualValidPlacement;
            CardDisplay.OnEndDrag_Event += ResetVisuals;
            CardDisplay.OnDoubleClick_Event += QuickPlacement;

            base.RegisterEvents();
        }

        protected override void UnregisterEvents()
        {
            CardDisplay.OnBeginDrag_Event -= VisualValidPlacement;
            CardDisplay.OnEndDrag_Event -= ResetVisuals;
            CardDisplay.OnDoubleClick_Event -= QuickPlacement;

            base.UnregisterEvents();
        }

        private bool IsFull()
        {
            return CardCount >= 13;
        }

        private void QuickPlacement(CardDisplay card,UndoAction someType)
        {
            if(card.CurrentPile.IsCardLastIndex(card) == false)
            {
                return;
            }

            var cardToDrop = new CardDisplay[] { card };

            if(IsValidDrop(cardToDrop))
            {
                var topCard = card.CurrentPile.GetCard(card);

                PlaceCard(topCard);

                topCard.PreviousPile.HandleTopCard(true);

                OnQuickPlacement(card, someType);

                OnFull_Event(this, IsFull());
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            var result = OnCardsDrop_Event();
            ValidateDrop(result);
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void ResetVisuals(CardDisplay card)
        {
            if(highlightResponse == null)
            {
                return;
            }

            highlightResponse.HoverOutAnimation();
        }

        private void VisualValidPlacement(CardDisplay card)
        {
            if(card.CurrentPile.IsCardLastIndex(card) == false)
            {
                return;
            }

            var cardToDrop = new CardDisplay[] { card };

            if(IsValidDrop(cardToDrop))
            {
                if(highlightResponse == null)
                {
                    return;
                }

                highlightResponse.HoverInAnimation();
                return;
            }
        }

        public bool IsValidDrop(CardDisplay[] cards)
        {
            if(cards == null)
            {
                return false;
            }

            if(cards.Length == 0)
            {
                return false;
            }

            var firstDroppedCard = cards[cards.Length - 1];
       
            if(cards.Length > 1)
            {
                return false;
            }

            if(cardsInContainer == null)
            {
                return false;
            }

            if(firstDroppedCard.Data.Suit != suitType)
            {
                return false;
            }

            if(cardsInContainer.Count == 0 && firstDroppedCard.Data.Value == EMPTY_PILE_START_VALUE)
            {
                return true;
            }

            if(cardsInContainer.Count == 0)
            {
                return false;
            }

            var topCard = cardsInContainer[cardsInContainer.Count - 1];

            if(topCard.Data.Value != firstDroppedCard.Data.Value - 1)
            {
                return false;
            }

            return true;
        }

        private void ValidateDrop(CardDisplay[] cards)
        {
            if(IsValidDrop(cards))
            {
                var topCard = cards[0];

                topCard.PreviousPile.HandleTopCard(true);

                var pileToReturn = topCard.PreviousPile;

                OnCardValidPlacement_Event(cards, new UndoAction(() =>
                {
                    topCard.CurrentPile.GetCard(topCard);

                    pileToReturn.PlaceCard(topCard);

                    topCard.CurrentPile.HandlePreviousCard(false);
                }));

                PlaceCards(cards);

                OnFull_Event(this, IsFull());

                return;
            }

            OnCardsInvalidPlacement_Event(cards);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}