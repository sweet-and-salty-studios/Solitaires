using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SweetAndSaltyStudios
{
	public class InGamePanel : UIPanel
	{
        #region VARIABLES

        private float gameTime;

#pragma warning disable 0649

        [SerializeField] private Button undoButton;
        [SerializeField] private TextMeshProUGUI movesText;
        [SerializeField] private TextMeshProUGUI timeText;
        private bool isGameTimeRunning;

#pragma warning restore 0649

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        protected override void Awake()
        {
            base.Awake();

            GameMaster.OnCheckMovesCurrentState_Event += GameMaster_OnMoveCreated;
        }

        private void OnDestroy()
        {
            GameMaster.OnCheckMovesCurrentState_Event -= GameMaster_OnMoveCreated;
        }

        private void Start()
        {
            undoButton.gameObject.SetActive(false);
        }

        private void Update()
        {
            if(isGameTimeRunning == false) return;

            gameTime += Time.deltaTime;
            timeText.text = gameTime.ToString("0");
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            LeanTween.scale(RectTransform, new Vector3(0, 0, 1), 0.1f)
            .setOnStart(() =>
            {
                CanvasGroup.blocksRaycasts = false;
            });

        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            LeanTween.scale(RectTransform, Vector3.one, 0.1f)
            .setOnStart(() =>
            {
                CanvasGroup.blocksRaycasts = false;
            })
            .setEaseOutCubic()
            .setOnComplete(() =>
            {
                CanvasGroup.blocksRaycasts = true;
            });
        }

        public override void OnDrag(PointerEventData eventData)
        {
            
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        protected override int StartOpenAnimation(float animationDuration)
        {
            isGameTimeRunning = true;
            return base.StartOpenAnimation(animationDuration);
        }

        protected override int StartCloseAnimation(float aniamtionDuration)
        {
            isGameTimeRunning = false;
            return base.StartCloseAnimation(aniamtionDuration);
        }

        private void GameMaster_OnMoveCreated(int moves)
        {
            movesText.text = moves.ToString();

            undoButton.gameObject.SetActive(moves != 0);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}