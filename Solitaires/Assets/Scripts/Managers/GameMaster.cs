using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SweetAndSaltyStudios
{
    public class GameMaster : MonoBehaviour
    {
        #region VARIABLES

        public static event Action<int> OnCheckMovesCurrentState_Event = delegate { };
        public static event Action<bool> OnCheckIfGameOver = delegate { };
        public static event Action OnGameOver = delegate { };
        public static event Action<Card[]> OnCardsCreated = delegate { };
      
        private Stack<UndoAction> undoActions = new Stack<UndoAction>();

        private List<Foundation_Pile> fullFoundationPiles = new List<Foundation_Pile>();

        private int moveCount;

        private CardData[] cardData;
        private Card cardPrefab;

        private Card[] createdCards;
        private Sprite[] cardBackSprites;

        private Transform gameArea;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            Initialize();

            RegisterEvents();
        }

        private void OnDestroy()
        {
            UnregisterEvents();
        }

        private void Start()
        {
            gameArea.gameObject.SetActive(false);
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void Initialize()
        {
            gameArea = GetComponentInChildren<Canvas>().transform.Find("GameArea");       

            cardData = Resources.LoadAll<CardData>("CardData/");
            cardBackSprites = Resources.LoadAll<Sprite>("Sprites/");
            cardPrefab = Resources.Load<Card>("Prefabs/CardPrefab");
        }

        private void RegisterEvents()
        {
            Foundation_Pile.OnFull_Event += CheckIfGameOver;

            Foundation_Pile.OnQuickPlacement += AddUndoAction;
            Foundation_Pile.OnCardValidPlacement_Event += AddUndoAction;
            Tableau_Pile.OnCardsValidPlacement_Event += AddUndoAction;
            Card.OnFlip_Event += AddUndoAction;
            Stock_Pile.OnCardDrawed_Event += AddUndoAction;
        }

        private void UnregisterEvents()
        {
            Foundation_Pile.OnFull_Event -= CheckIfGameOver;

            Foundation_Pile.OnQuickPlacement -= AddUndoAction;
            Foundation_Pile.OnCardValidPlacement_Event -= AddUndoAction;
            Tableau_Pile.OnCardsValidPlacement_Event -= AddUndoAction;
            Card.OnFlip_Event -= AddUndoAction;
            Stock_Pile.OnCardDrawed_Event -= AddUndoAction;
        }

        private void AddUndoAction(Card[] cards, UndoAction undoAction)
        {
            // We nee only create one undo action for multiple cards....?
            //for(int i = 0; i < cards.Length; i++)
            //{
            //    AddUndoAction(cards[i], undoAction);
            //}

            AddUndoAction(cards[0], undoAction);
        }

        private void AddUndoAction(Card card, UndoAction undoAction)
        {
            undoActions.Push(undoAction);

            moveCount++;

            OnCheckMovesCurrentState_Event(moveCount);
        }

        private Sprite GetRandomSprite(Sprite[] sprites)
        {
            if(sprites == null || sprites.Length == 0)
            {
                return null;
            }

            return sprites[UnityEngine.Random.Range(0, sprites.Length - 1)];
        }

        private Card[] CreateCards(CardData[] cardData, bool isTurned = false)
        {
            var result = new Card[cardData.Length];

            var cardBackSprite = GetRandomSprite(cardBackSprites);

            for(int i = 0; i < cardData.Length; i++)
            {
                cardData[i].BackSprite = cardBackSprite;

                var newCard = Instantiate(cardPrefab, gameArea.transform);
                newCard.Initialize(cardData[i], isTurned);

                result[i] = newCard;
            }

            return result;
        }

        public void QuitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void PlayButton()
        {
            createdCards = CreateCards(cardData);

            gameArea.gameObject.SetActive(true);

            OnCardsCreated(createdCards);
        }

        public void RestartButton()
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadScene(currentSceneIndex);
        }

        public void UndoMove()
        {
            if(undoActions == null)
            {
                return;
            }

            if(undoActions.Count == 0)
            {
                Debug.Log("No moves to 'Undo'");
                return;
            }

            var previousMove = undoActions.Pop();

            previousMove.RedoAction();

            moveCount--;

            OnCheckMovesCurrentState_Event(moveCount);
        }

        private bool CheckIfGameOver(Foundation_Pile foundation_Pile, bool isFull)
        {
            // REFACTOR

            if(isFull)
            {
                fullFoundationPiles.Add(foundation_Pile);
                Debug.LogError(foundation_Pile.name + " ADDED");
            }
            else
            {
                if(fullFoundationPiles.Remove(foundation_Pile))
                {
                    Debug.LogError(foundation_Pile.name + " Removed");
                }
            }

            if(fullFoundationPiles.Count >= 4)
            {
                Debug.LogError("YOU ARE WINNER!");
                OnGameOver();
                return true;
            }

            return false;
        }

        #endregion CUSTOM_FUNCTIONS
    }
}