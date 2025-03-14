using System;
using System.Collections.Generic;
using Card;
using Infrastructure.Extension;
using Infrastructure.Services;
using Infrastructure.Services.Messeges;
using UnityEngine;

namespace UI
{
    public class BuildMediator: MonoBehaviour, IMediator, IDisposable
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private BuildWindow _buildWindow;
        private Messenger _messenger;

        public event Action<IMediator> OnCleanUp;
        public GameObject GameObject => gameObject;

        public void Construct(List<BuildConfig> buildConfigs, Messenger messenger)
        {
            _messenger = messenger;
            _buildWindow.Init(buildConfigs);
            _buildWindow.OnSelectedBuild += SelectBuild;
        }

        private void SelectBuild(BuildType buildType)
        {
            _messenger.Pub(new SelectBuild(buildType));
        }

        public void Show() => _canvasGroup.Show();

        public void Hide() => _canvasGroup.Hide();

        public void Dispose()
        {
            _buildWindow.OnSelectedBuild -= SelectBuild;
            _buildWindow?.Dispose();
        }
    }
}