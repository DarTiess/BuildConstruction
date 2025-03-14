using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Card
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Ground : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private GroundConfig _config;
        private Vector2 _mousePosition;
        private float _offsetX;
        private float _offsetY;
        private bool _isSelected;
        private Camera _camera;
        private Sprite _currenIcon;
        private bool _onComparing;
        private Vector3 _startPosition;
        private float _animateDuration;

        public Sprite CurrentIcon => _currenIcon;
        public event Action GroundUpgrade;

        public event Action<int, BuildType> CollectCard;

        private void OnMouseDown()
        {
            _isSelected = false;
            Vector3 mousePos = Mouse.current.position.ReadValue();   
            mousePos.z=_camera.nearClipPlane;
            _offsetX = _camera.ScreenToWorldPoint( mousePos).x-transform.position.x;
            _offsetY = _camera.ScreenToWorldPoint(mousePos).y-transform.position.y;
        }

        private void OnMouseDrag()
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z=_camera.nearClipPlane;
            _mousePosition = _camera.ScreenToWorldPoint(mousePos);
            transform.position = new Vector3(_mousePosition.x -_offsetX, _mousePosition.y-_offsetY);
        }

        private void OnMouseUp()
        {
            _isSelected = true;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out Ground card) && _isSelected && !_onComparing)
            {
              //  TryMergeCard(ground);
            }
        }

        public void Initialize(GroundConfig config)
        {
            _config = config;
            // _animateDuration = config.AnimateDuration;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _camera=Camera.main;
            _startPosition = transform.position;
            _onComparing = false;
            _spriteRenderer.sprite = _config.Icon;
        }

        public void HideCard()
        {
            transform.DOScale(Vector3.zero, _animateDuration).SetEase(Ease.InBounce)
                .OnComplete(() =>
                {
                    // CollectCard?.Invoke(_config.Price, _config.BuildType);
                    gameObject.SetActive(false);
                });
        }

        private void TryMergeCard(Ground ground)
        {
            if (HasSameConfig(ground.CurrentIcon))
            {
                MergeCard(ground);
            }
            else
            {
                transform.position = _startPosition;
            }
        }

        private void MergeCard(Ground ground)
        {
            _onComparing = true;
            ground.ChangeIconLevel();
            gameObject.SetActive(false);
        }

        private void ChangeIconLevel()
        {
           GroundUpgrade?.Invoke();
        }

        private bool HasSameConfig(Sprite icon)
        {
            if (gameObject.activeInHierarchy)
            {
                return _currenIcon.Equals(icon);
            }
            return false;
        }
    }
}