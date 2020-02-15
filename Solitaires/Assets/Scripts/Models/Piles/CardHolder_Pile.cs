using System;
using UnityEngine;

namespace SweetAndSaltyStudios
{
    public class CardHolder_Pile : Pile
    {
        #region VARIABLES

        public static Action OnCardReset_Event = delegate { };

        private CanvasGroup canvasGroup;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        protected override void Awake()
        {
            base.Awake();

            canvasGroup = GetComponent<CanvasGroup>();
        }

        protected override void Start()
        {
            base.Start();

            backgroundImage.enabled = false;
            CardDisplayCounter.enabled = false;
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        protected override void RegisterEvents()
        {
            CardDisplay.OnBeginDrag_Event += StartDrag;
            CardDisplay.OnDrag_Event += Drag;
            CardDisplay.OnEndDrag_Event += EndDrag;

            Foundation_Pile.OnCardsDrop_Event += GetAllCards;
            Foundation_Pile.OnCardsInvalidPlacement_Event += ResetCardsToPreviousPile;
            Tableau_Pile.OnCardsDrop_Event += GetAllCards;
            Tableau_Pile.OnCardsInvalidPlacement_Event += ResetCardsToPreviousPile;

            base.RegisterEvents();
        }

        protected override void UnregisterEvents()
        {
            CardDisplay.OnBeginDrag_Event -= StartDrag;
            CardDisplay.OnDrag_Event -= Drag;
            CardDisplay.OnEndDrag_Event -= EndDrag;

            Foundation_Pile.OnCardsDrop_Event -= GetAllCards;
            Foundation_Pile.OnCardsInvalidPlacement_Event -= ResetCardsToPreviousPile;

            Tableau_Pile.OnCardsDrop_Event -= GetAllCards;
            Tableau_Pile.OnCardsInvalidPlacement_Event -= ResetCardsToPreviousPile;

            base.UnregisterEvents();
        }

        private void ResetCardsToPreviousPile(CardDisplay[] cards)
        {
            if(cards == null || cards.Length == 0)
            {
                return;
            }

            for(int i = cards.Length - 1; i >= 0; i--)
            {
                cards[i].PreviousPile.PlaceCard(cards[i]);
                print("Current pile: " + cards[i].CurrentPile.name);
                print("Previous pile: " + cards[i].PreviousPile.name);
            }

            OnCardReset_Event();
        }

        public void StartDrag(CardDisplay card)
        {
            var result = card.CurrentPile.GetValidCards(card);

            if(result == null)
            {
                return;
            }

            for(int i = 0; i < result.Length; i++)
            {
                PlaceCard(result[i]);
            }

            Show(card.transform.position);
        }

        private void Show(Vector2 position)
        {
            backgroundImage.enabled = true;
            CardDisplayCounter.enabled = true;

            transform.position = position;

            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        public void Drag(Vector2 position)
        {
            RectTransform.anchoredPosition += position;
        }

        public void EndDrag(CardDisplay card)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;

            backgroundImage.enabled = false;
            CardDisplayCounter.enabled = false;

            if(CardCount == 0)
            {
                return;
            }

            ResetCardsToPreviousPile(GetAllCards());
        }

        #endregion CUSTOM_FUNCTIONS
    }
}