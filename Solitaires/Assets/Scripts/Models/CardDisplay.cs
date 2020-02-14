using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SweetAndSaltyStudios
{
    public class CardDisplay : 
    MonoBehaviour,
    IDraggable,
    IClickable
	{
        #region VARIABLES

        public static event Action<CardDisplay> OnBeginDrag_Event = delegate { };
        public static event Action<Vector2> OnDrag_Event = delegate { };
        public static event Action<CardDisplay> OnEndDrag_Event = delegate { };
        public static event Action<CardDisplay, UndoAction> OnDoubleClick_Event = delegate { };
        public static event Action<CardDisplay, UndoAction> OnFlip_Event = delegate { };

        private Image backgroundImage;
        private CanvasGroup canvasGroup;
        private IHighlightResponse highlightResponse;

        private bool initialized;

        #endregion VARIABLES

        #region PROPERTIES

        public CardData Data
        {
            get;
            private set;
        }

        public Pile CurrentPile
        {
            get;
            private set;
        }

        public Pile PreviousPile
        {
            get;
            private set;
        }

        public bool IsTurned
        {
            get;
            private set;
        }

        public bool IsInteractable
        {
            get;
            private set;
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            backgroundImage = GetComponentInChildren<Image>();
            highlightResponse = GetComponentInChildren<IHighlightResponse>();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public void Initialize(CardData cardData, bool isTurned = false)
        {
            if(initialized)
            {
                return;
            }

            Data = cardData;

            canvasGroup = GetComponent<CanvasGroup>();
            backgroundImage = GetComponentInChildren<Image>();
            name = cardData.name;

            backgroundImage.sprite = isTurned ? Data.FrontSprite : Data.BackSprite;
            IsTurned = isTurned;
            SetInteractability(isTurned);

            initialized = true;
        }

        public void SetInteractability(bool isInteractable)
        {
            IsInteractable = isInteractable;
            canvasGroup.interactable = isInteractable;
            canvasGroup.blocksRaycasts = isInteractable;
        }

        public void SetPileAndParent(Pile newPile)
        {
            PreviousPile = CurrentPile;
            CurrentPile = newPile;

            transform.SetParent(newPile.Content);
        }

        public void Flip(bool turnFaceUp, bool interactability)
        {
            backgroundImage.sprite = turnFaceUp ? Data.FrontSprite : Data.BackSprite;
            IsTurned = turnFaceUp;
            SetInteractability(interactability);
        }

        public IEnumerator IFlip(bool turnFaceUp, float flipSpeed = 0.25f)
        {
            var isAnimating = true;

            LeanTween.scaleX(gameObject, 0, flipSpeed)
                .setEaseInBack()
                .setOnStart(() =>
                {
                    SetInteractability(false);
                })
                .setOnComplete(() =>
                {
                    backgroundImage.sprite = turnFaceUp ? Data.FrontSprite : Data.BackSprite;

                    LeanTween.scaleX(gameObject, 1, flipSpeed)
                    .setOnComplete(() =>
                    {
                        IsTurned = turnFaceUp;
                        SetInteractability(turnFaceUp);

                        isAnimating = false;
                    })
                    .setEaseOutCubic();
                });

            yield return new WaitWhile(() => isAnimating);
        }

        public IEnumerator IMoveToPile(Pile newPile, Vector2 targetPosition, float moveSpeed = 0.25f)
        {
            var isAnimating = true;

            LeanTween.move(gameObject, targetPosition, moveSpeed).setOnStart(() =>
            {
                transform.SetParent(transform.parent);
            })
            .setEaseLinear()
            .setFrom(transform.position)
            .setOnComplete(() =>
            {
                isAnimating = false;
            });

            yield return new WaitWhile(() => isAnimating);
        }

        public void OnPointerDown()
        {
            if(highlightResponse == null) { return; }

            highlightResponse.HoverInAnimation();
        }

        public void OnPointerClick()
        {
            if(IsTurned == false)
            {
                Flip(true, true);

                OnFlip_Event(this, new UndoAction(() =>
                {
                    Flip(false, IsTurned);
                }));
            }
        }

        public void OnPointerUp()
        {
            if(highlightResponse == null) { return; }

            highlightResponse.HoverOutAnimation();
        }

        public void OnDoubleClick()
        {
            var undoAction = new UndoAction(() =>
            {
                CurrentPile.GetCard(this);

                PreviousPile.PlaceCard(this);

                CurrentPile.HandlePreviousCard(false);
            });

            OnDoubleClick_Event(this, undoAction);
        }

        public void OnBeginDrag()
        {
            if(IsTurned == false)
            {
                Debug.LogWarning($"Card {name}: On Begin Drag fail check -> but was not turned... Most likely we were under animation");
                return;
            }

            OnBeginDrag_Event(this);
        }

        public void OnDrag(Vector3 deltaPosition)
        {
            OnDrag_Event(deltaPosition);
        }

        public void OnEndDrag()
        {
            OnEndDrag_Event(this);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}