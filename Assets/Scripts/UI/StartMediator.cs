using System;
using Infrastructure.Extension;
using Infrastructure.Services;
using UnityEngine;

namespace UI
{
    public class StartMediator: MonoBehaviour, IMediator, IDisposable
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        public event Action<IMediator> OnCleanUp;
        public GameObject GameObject => gameObject;

        public void Construct(Messenger messenger)
        {
        }

        public void Show() => _canvasGroup.Show();

        public void Hide() => _canvasGroup.Hide();

        public void Dispose()
        {
        }
    }
}