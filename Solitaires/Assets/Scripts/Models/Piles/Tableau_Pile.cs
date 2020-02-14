using System;
using UnityEngine.EventSystems;

namespace SweetAndSaltyStudios
{
    public class Tableau_Pile : Pile, IDropHandler
    {
        #region VARIABLES

        public static event Func<CardDisplay[]> OnCardsDrop_Event = delegate { return null; };
        public static event Action<CardDisplay[], UndoAction> OnCardsValidPlacement_Event = delegate { };
        public static event Action<CardDisplay[]> OnCardsInvalidPlacement_Event = delegate { };

        private const int EMPTY_PILE_START_VALUE = 13;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        public void OnDrop(PointerEventData eventData)
        {
            var result = OnCardsDrop_Event();
            ValidateDrop(result);
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        protected override void RegisterEvents()
        {
            CardDisplay.OnBeginDrag_Event += VisualValidPlacement;
            CardDisplay.OnEndDrag_Event += ResetVisuals;

            base.RegisterEvents();
        }

        protected override void UnregisterEvents()
        {
            CardDisplay.OnBeginDrag_Event -= VisualValidPlacement;
            CardDisplay.OnEndDrag_Event -= ResetVisuals;

            base.UnregisterEvents();
        }

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
            if(IsValidDrop(new CardDisplay[] { card }))
            {
                if(highlightResponse == null)
                {
                    return;
                }

                highlightResponse.HoverInAnimation();
                return;
            }

            return;
        }

        protected bool IsValidDrop(CardDisplay[] cards)
        {
            if(cards == null)
            {
                return false;
            }

            if(cards.Length == 0)
            {
                return false;
            }

            var firstCard = cards[cards.Length - 1];

            if(cardsInContainer == null)
            {
                return false;
            }

            if(cardsInContainer.Count == 0 && firstCard.Data.Value == EMPTY_PILE_START_VALUE)
            {
                return true;
            }

            if(cardsInContainer.Count == 0)
            {
                return false;
            }

            var topCard = cardsInContainer[cardsInContainer.Count - 1];

            if(topCard.IsTurned == false)
            {
                return false;
            }

            if(topCard.Data.IsRed == firstCard.Data.IsRed)
            {
                return false;
            }

            if(topCard.Data.Value != firstCard.Data.Value + 1)
            {
                return false;
            }

            return true;
        }

        private void ValidateDrop(CardDisplay[] cards)
        {
            var canDrop = IsValidDrop(cards);

            if(canDrop)
            {
                var topCard = cards[0];

                topCard.PreviousPile.HandleTopCard(true);

                CardDisplay card;
                Pile pileToReturn;

                pileToReturn = topCard.PreviousPile;

                var undoAction = new UndoAction(() =>
                {
                    for(int i = cards.Length - 1; i >= 0; i--)
                    {
                        //Debug.Log("MOVE");

                        card = cards[i];

                        card.CurrentPile.GetCard(card);
                        pileToReturn.PlaceCard(card);
                        card.CurrentPile.HandlePreviousCard(false);
                    }
                });

                OnCardsValidPlacement_Event(cards, undoAction);

                PlaceCards(cards);

                return;
            }

            OnCardsInvalidPlacement_Event(cards);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}