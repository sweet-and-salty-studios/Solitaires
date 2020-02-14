using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SweetAndSaltyStudios
{
    public class Card : MonoBehaviour, IDraggable, IInteractable
	{
        #region VARIABLES

        public static event Action<Card> OnBeginDrag_Event = delegate { };
        public static event Action<Vector2> OnDrag_Event = delegate { };
        public static event Action<Card> OnEndDrag_Event = delegate { };
        public static event Action<Card, UndoAction> OnDoubleClick_Event = delegate { };
        public static event Action<Card, UndoAction> OnFlip_Event = delegate { };

        private Image backgroundImage;
        private IHighlightResponse highlightResponse;
        private CanvasGroup canvasGroup;

        private bool initialized;
        private float lastClick;
        private readonly float interval = 0.2f;

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
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(IsTurned == false)
            {
                Debug.LogWarning($"Card {name}: On Begin Drag fail check -> but was not turned... Most likely we were under animation");
                return;
            }

            OnBeginDrag_Event(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDrag_Event(eventData.delta);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDrag_Event(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(highlightResponse == null)
            {
                return;
            }

            highlightResponse.HoverInAnimation();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(IsTurned)
            {
#if UNITY_EDITOR
                if(IsDoubleTap(eventData))
                {
                    var undoAction = new UndoAction(() =>
                    {
                        //Debug.Log("MOVE");
                        CurrentPile.GetCard(this);

                        PreviousPile.PlaceCard(this);

                        CurrentPile.HandlePreviousCard(false);
                    });

                    OnDoubleClick_Event(this, undoAction);
                }
#else
                if(IsDoubleTap())
                {
                    OnDoubleClick_Event(this, undoAction);
                }
#endif
            }
            else
            {
                Flip(true, true);

                var undoAction = new UndoAction(() =>
                {
                    //Debug.Log("FLIP");

                    Flip(false, IsTurned);
                });

                OnFlip_Event(this, undoAction);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(highlightResponse == null)
            {
                return;
            }

            highlightResponse.HoverOutAnimation();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private bool IsDoubleTap(PointerEventData eventData)
        {
            return eventData.clickCount == 2;
        }

        private bool IsDoubleTap()
        {
            if((lastClick + interval) > Time.time)
            {
                return true;
            }

            lastClick = Time.time;

            return false;
        }

        public void Initialize(CardData cardData, bool isTurned = false)
        {
            if(initialized)
            {
                return;
            }

            Data = cardData;

            canvasGroup = GetComponent<CanvasGroup>();
            backgroundImage = GetComponentInChildren<Image>();
            highlightResponse = GetComponentInChildren<IHighlightResponse>();

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

        #endregion CUSTOM_FUNCTIONS
    }
}