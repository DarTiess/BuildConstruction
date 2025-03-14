using System;
using Infrastructure.Extension;
using UnityEngine;

namespace UI
{
    public class StartMediator: MonoBehaviour, IMediator, IDisposable
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private StartWindow _startWindow;
        public event Action<IMediator> OnCleanUp;
        public GameObject GameObject => gameObject;
        public event Action OnStartGame;

        public void Construct()
        {
           
        }

        private void Start()
        {
            _startWindow.Start += StartGame;
        }

        private void StartGame()
        {
           OnStartGame?.Invoke();
        }

        public void Show() => _canvasGroup.Show();

        public void Hide() => _canvasGroup.Hide();

        public void Dispose()
        {
            _startWindow.Start -= StartGame;
        }
    }
}