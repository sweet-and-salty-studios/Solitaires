using System.Collections;
using UnityEngine;

namespace SweetAndSaltyStudios
{
	public class UIManager : MonoBehaviour
	{
        #region VARIABLES

        private UIPanel currentPanel;
        private Coroutine iOpenPanel_Coroutine;
        private Coroutine iChangePanel_Coroutine;

#pragma warning disable 0649

        [SerializeField] private  UIPanel startingPanel;
        [SerializeField] private GameOverPanel gameOverPanel;
        [SerializeField] private MusicPlayerPanel musicPlayerPanel;

#pragma warning restore 0649

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            var allPanels = FindObjectsOfType<UIPanel>();

            for(int i = 0; i < allPanels.Length; i++)
            {
                allPanels[i].gameObject.SetActive(false);
            }

            GameMaster.OnGameOver += GameMaster_OnGameOver;
        }

        private void OnDestroy()
        {
            GameMaster.OnGameOver -= GameMaster_OnGameOver;
        }

        private void Start()
        {
            ChangePanelWithDelay(startingPanel, 1);

            OpenPanel(musicPlayerPanel, 2);
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void GameMaster_OnGameOver()
        {
            ChangePanel(gameOverPanel);
        }

        public void OpenPanel(UIPanel panel, float delay = 0)
        {
            if(iOpenPanel_Coroutine != null)
            {
                return;
            }

            iOpenPanel_Coroutine = StartCoroutine(IOpenPanel(panel, delay));
        }

        public void ChangePanel(UIPanel panel)
        {
            if(iChangePanel_Coroutine != null)
            {
                return;
            }

            iChangePanel_Coroutine = StartCoroutine(IChangePanel(panel));
        }

        public void ChangePanelWithDelay(UIPanel panel, float delay)
        {
            if(iChangePanel_Coroutine != null)
            {
                return;
            }

            iChangePanel_Coroutine = StartCoroutine(IChangePanel(panel, delay));
        }

        private IEnumerator IOpenPanel(UIPanel panel, float delay = 0)
        {
            if(delay != 0)
            {
                yield return new WaitForSeconds(delay);
            }

            if(panel != null)
            {
                panel.gameObject.SetActive(true);

                yield return panel.Open();
            }

            iOpenPanel_Coroutine = null;
        }

        private IEnumerator IChangePanel(UIPanel panel, float delay = 0)
        {
            if(delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            if(currentPanel != null)
            {
                yield return currentPanel.Close();
                currentPanel.gameObject.SetActive(false);
            }

            yield return IOpenPanel(panel);

            currentPanel = panel;

            iChangePanel_Coroutine = null;
        }

        #endregion CUSTOM_FUNCTIONS
	}
}