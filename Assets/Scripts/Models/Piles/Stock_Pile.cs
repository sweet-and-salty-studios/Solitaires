using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetAndSaltyStudios
{
    public class Stock_Pile : 
    Pile,
    IClickable
    {
        #region VARIABLES

        [Space]
        [Header("Settings")]
        public SOLITAIRE_TYPE SolitaireType;
        public bool IsAnimating;

        [Range(0, 10)] public float CardMovementDuration;
        [Range(0, 10)] public float CardFlipDuration;

        public static event Action<CardDisplay, UndoAction> OnCardDrawed_Event = delegate { };

        private Coroutine iDeal_Coroutine;
        private Coroutine iDrawCard_Coroutine;
        private Coroutine iResetCards_Coroutine;

#pragma warning disable 0649
        [SerializeField] private Tableau_Pile[] tableau_Piles;
        [SerializeField] private Waste_Pile waste;
#pragma warning restore 0649

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS     
        
        protected override void RegisterEvents()
        {
            GameMaster.OnCardsCreated += DealCards;

            base.RegisterEvents();
        }

        protected override void UnregisterEvents()
        {
            GameMaster.OnCardsCreated -= DealCards;

            base.UnregisterEvents();
        }

        private void DealCards(CardDisplay[] createdCards)
        {
            if(iDeal_Coroutine != null)
            {
                return;
            }

            iDeal_Coroutine = StartCoroutine(IDealCards(createdCards));
        }

        private void DrawCard()
        {
            if(iDrawCard_Coroutine != null)
            {
                return;
            }

            if(CardCount == 0 && waste.CardCount == 0)
            {
                Debug.LogError("STOCK AND WASTE ARE EMPTY");
                return;
            }

            if(CardCount == 0)
            {
                if(iResetCards_Coroutine != null)
                {
                    return;
                }

                iResetCards_Coroutine = StartCoroutine(IResetCards());

                return;
            }

            iDrawCard_Coroutine = StartCoroutine(IDrawCard());
        }

        private void Shuffle<T>(IList<T> listToShuffle)
        {
            var n = listToShuffle.Count;

            var random = new System.Random();

            while(n > 1)
            {
                n--;

                var k = random.Next(n + 1);

                var temp = listToShuffle[k];
                listToShuffle[k] = listToShuffle[n];
                listToShuffle[n] = temp;
            }
        }

        private IEnumerator IDealCards(CardDisplay[] cards)
        {
            Shuffle(cards);

            PlaceCards(cards);

            var pilesToDeal = new Queue<Pile>(tableau_Piles);

            var targetPosition = Vector2.zero;
            var contentOffset = new Vector3(0, -30);
            var currentOffset = Vector3.zero;

            while(pilesToDeal.Count > 0)
            {
                for(int i = 0; i < tableau_Piles.Length; i++)
                {
                    if(tableau_Piles[i].CardCount == i + 1)
                    {
                        continue;
                    }

                    var topCard = GetTopCard();

                    if(IsAnimating)
                    {
                        targetPosition = tableau_Piles[i].RectTransform.position + currentOffset;

                        yield return topCard.IMoveToPile(tableau_Piles[i], targetPosition, CardMovementDuration);
                    }

                    tableau_Piles[i].PlaceCard(topCard);

                    if(tableau_Piles[i].CardCount == i + 1)
                    {
                        if(IsAnimating)
                        {
                            yield return topCard.IFlip(true, CardFlipDuration);
                        }
                        else
                        {
                            topCard.Flip(true, true);
                        }
                    }
                }

                currentOffset += contentOffset;

                pilesToDeal.Dequeue();
            }

            iDeal_Coroutine = null;
        }

        private IEnumerator IResetCards()
        {
            var cards = waste.GetAllCards();

            if(IsAnimating)
            {
                var isAnimatingScale = false;
                var isAnimatingMovement = false;

                yield return new WaitUntil(() => isAnimatingScale == false && isAnimatingMovement == false);
            }

            CardDisplay card = null;
            var pileToReturn = cards[0].CurrentPile;

            for(int i = 0; i < cards.Length; i++)
            {
                card = cards[i];
                card.Flip(false, false);
                PlaceCard(card);
            }

            var undoAction = new UndoAction(() =>
            {
                for(int i = cards.Length - 1; i >= 0; i--)
                {
                //for(int i = 0; i < cards.Length; i++)
                    card = cards[i].CurrentPile.GetCard(cards[i]);
                    pileToReturn.PlaceCard(card);

                    card.Flip(true, true);

                    card.CurrentPile.HandlePreviousCard(false);
                }
            });

            OnCardDrawed_Event(card, undoAction);

            iResetCards_Coroutine = null;
        }

        private IEnumerator IDrawCard()
        {
            var card = GetTopCard();

            if(IsAnimating)
            {
                yield return card.IMoveToPile(waste, waste.transform.position, CardMovementDuration);
            }

            waste.PlaceCard(card);

            if(IsAnimating)
            {
                yield return card.IFlip(true, CardFlipDuration);
            }
            else
            {
                card.Flip(true, true);
            }

            var pileToReturn = card.PreviousPile;

            var undoAction = new UndoAction(() =>
            {
                // Debug.Log("MOVE AND FLIP");

                card.CurrentPile.GetCard(card);
                pileToReturn.PlaceCard(card);

                card.Flip(false, card.IsTurned == false);

                card.CurrentPile.HandlePreviousCard(false);

            });

            OnCardDrawed_Event(card, undoAction);

            iDrawCard_Coroutine = null;
        }

        public void OnPointerDown()
        {
            if(highlightResponse == null) { return; }

            highlightResponse.OnHoverIn();
        }

        public void OnPointerClick()
        {
            DrawCard();
        }

        public void OnPointerUp()
        {
            if(highlightResponse == null) { return; }

            highlightResponse.OnHoverIn();
        }

        public void OnDoubleClick()
        {

        }

        #endregion CUSTOM_FUNCTIONS
    }
}