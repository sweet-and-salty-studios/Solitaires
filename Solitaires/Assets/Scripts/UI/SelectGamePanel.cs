using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SweetAndSaltyStudios
{
	public class SelectGamePanel : UIPanel
	{
        #region VARIABLES

        private Button buttonTemplatePrefab;
        private Transform gameButtonsContainer;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        protected override void Awake()
        {
            base.Awake();

            buttonTemplatePrefab = Resources.Load<Button>("Prefabs/ButtonTemplate");

            // WTF...

            gameButtonsContainer = transform.Find("Content")
                .transform.Find("ScrollableArea")
                .transform.Find("Mask")
                .transform.Find("GameButtons");
        }

        private void Start()
        {
            CreateGameSelectionButtons();
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void CreateGameSelectionButtons()
        {
            var solitareNameTypes = Enum.GetNames(typeof(SOLITAIRE_TYPE));

            for(int i = 0; i < solitareNameTypes.Length; i++)
            {
                var newButton = Instantiate(buttonTemplatePrefab, gameButtonsContainer);

                solitareNameTypes[i] = solitareNameTypes[i].Replace('_', ' ');

                newButton.name = $"{solitareNameTypes[i]}";
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = newButton.name;
            } 
        }

        #endregion CUSTOM_FUNCTIONS
    }
}