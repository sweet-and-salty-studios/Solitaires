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
        public static event Action<CardDisplay[]> OnCardsCreated = delegate { };
      
        private Stack<UndoAction> undoActions = new Stack<UndoAction>();

        private int moveCount;

#pragma warning disable 0649

        [SerializeField] private CardDisplay cardPrefab;
        [SerializeField] private CardData[] cardData;
        [SerializeField] private Sprite[] cardBackSprites;

#pragma warning restore 0649

        private CardDisplay[] createdCards;

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
        }

        private void RegisterEvents()
        {
            Foundation_Pile.OnQuickPlacement += AddUndoAction;
            Foundation_Pile.OnCardValidPlacement_Event += AddUndoAction;
            Tableau_Pile.OnCardsValidPlacement_Event += AddUndoAction;
            CardDisplay.OnFlip_Event += AddUndoAction;
            Stock_Pile.OnCardDrawed_Event += AddUndoAction;
        }

        private void UnregisterEvents()
        {
            Foundation_Pile.OnQuickPlacement -= AddUndoAction;
            Foundation_Pile.OnCardValidPlacement_Event -= AddUndoAction;
            Tableau_Pile.OnCardsValidPlacement_Event -= AddUndoAction;
            CardDisplay.OnFlip_Event -= AddUndoAction;
            Stock_Pile.OnCardDrawed_Event -= AddUndoAction;
        }

        private void AddUndoAction(CardDisplay[] cards, UndoAction undoAction)
        {
            AddUndoAction(cards[0], undoAction);
        }

        private void AddUndoAction(CardDisplay card, UndoAction undoAction)
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

        private CardDisplay[] CreateCards(CardData[] cardData, bool isTurned = false)
        {
            var result = new CardDisplay[cardData.Length];

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

        #endregion CUSTOM_FUNCTIONS
    }
}