using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBased
{
    public class GameUI : MonoBehaviour
    {
        public Button startButton;
        public TextMeshProUGUI youLoseText;
        public TextMeshProUGUI youWinText;
        public GameObject actionButtons;
        public Button attackButton;
        public Button skipButton;

        private void Awake()
        {
            startButton.gameObject.SetActive(true);
            youLoseText.gameObject.SetActive(false);
            youWinText.gameObject.SetActive(false);
            actionButtons.gameObject.SetActive(false);
        }
    }
}

