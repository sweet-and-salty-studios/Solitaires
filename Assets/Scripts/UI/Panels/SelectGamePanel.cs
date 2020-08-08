using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SweetAndSaltyStudios
{
	public class SelectGamePanel : UIPanel
	{
        #region VARIABLES

#pragma warning disable 0649

        [SerializeField] private Button buttonTemplatePrefab;
        [SerializeField] private Transform gameButtonsContainer;

#pragma warning restore 0649

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

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