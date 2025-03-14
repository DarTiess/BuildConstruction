using System;
using Infrastructure.Extension;
using UnityEngine;

namespace UI
{
    public class BuildMediator: MonoBehaviour, IMediator, IDisposable
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public event Action<IMediator> OnCleanUp;
        public GameObject GameObject => gameObject;

        public void Construct()
        {
            
        }

        public void Show() => _canvasGroup.Show();

        public void Hide() => _canvasGroup.Hide();

        public void Dispose()
        {
        }
    }
}