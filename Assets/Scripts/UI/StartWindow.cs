using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartWindow: MonoBehaviour
    {
        [SerializeField] private Button _startButton;

        public event Action Start;

        private void OnEnable()
        {
            _startButton.onClick.AddListener(StartGame);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(StartGame);

        }

        private void StartGame()
        {
            Start?.Invoke();
        }
    }
}