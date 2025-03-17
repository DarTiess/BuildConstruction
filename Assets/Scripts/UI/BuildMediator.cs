using System;
using System.Collections.Generic;
using Data;
using Grid.Ground;
using Messeges;
using Messenger;
using UI.Extension;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class BuildMediator: MonoBehaviour, IMediator, IDisposable
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private BuildWindow _buildWindow;
        private IMessenger _messenger;
        private bool _onDelete;

        public event Action<IMediator> OnCleanUp;
        public GameObject GameObject => gameObject;

        public void Construct(List<BuildConfig> buildConfigs, IMessenger messenger)
        {
            _messenger = messenger;
            _buildWindow.Init(buildConfigs);
            _buildWindow.OnSelectedBuild += SelectBuild;
            _buildWindow.OnDeleteBuild += TryDeleteBuild;
            _messenger.Sub<BuildInstalled>(BuildPlaced);
        }
        private void Update()
        {
            if(!_onDelete)
                return;
            if (Mouse.current.leftButton.isPressed)
            {
                TryDeleteBuilding(Mouse.current.position.ReadValue());
            }
        }
        private void TryDeleteBuilding(Vector2 mousePosition)
        {
           Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
            worldPosition.z = 0;

            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if (hit.collider != null && hit.collider.TryGetComponent(out Ground ground))
            {
                if (ground.IsOccupied)
                {
                    Debug.Log($"Удаляем здание ");
                    ground.FreeGround();
                    Show();
                    _onDelete = false; 
                }
            }
        }
        private void TryDeleteBuild()
        {
            Hide();
            _onDelete = true;
        }

        private void BuildPlaced(BuildInstalled obj)
        {
            Show();
        }

        private void SelectBuild(BuildType buildType)
        {
            Hide();
            _messenger.Pub(new SelectBuild(buildType));
        }

        public void Show() => _canvasGroup.Show();

        public void Hide() => _canvasGroup.Hide();

        public void Dispose()
        {
            _buildWindow.OnDeleteBuild -= TryDeleteBuild;
            _buildWindow.OnSelectedBuild -= SelectBuild;
            _messenger.Unsub<BuildInstalled>(BuildPlaced);
            _buildWindow?.Dispose();
        }
    }
}