using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Data
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private BoxCollider2D _collider;
        private BuildType _buildType;
        private bool _isActive;
        private Camera _camera;
        private bool _isPlaced;

        public BuildType Type => _buildType;
        public Action<Vector3, Building> PlacedSaves;

        public event Action<Vector3, Building> Placed;
        public event Action<Building> Finished;

        private void Update()
        {
            if (!_isActive)
                return;
            if (_isPlaced)
                return;

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition =
                _camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, _camera.nearClipPlane));
            worldPosition.z = 0;

            Vector3 snappedPosition = new Vector3(
                Mathf.Round(worldPosition.x / 1.1f) * 1.1f,
                Mathf.Round(worldPosition.y / 1.1f) * 1.1f,
                0
            );

            transform.position = snappedPosition;

            if (Mouse.current.leftButton.isPressed)
            {
                Placed?.Invoke(transform.position, this);
            }
        }

        public void SetType(BuildType buildType, Sprite icon, Camera camera)
        {
            _buildType = buildType;
            _spriteRenderer.sprite = icon;
            _camera = camera;
            _isActive = true;
        }

        public void SetSaves(BuildType buildType, Sprite icon, Vector2Int positionBuild)
        {
            Debug.Log("SetSaves "+positionBuild.x+", "+positionBuild.y);
            _buildType = buildType;
            _spriteRenderer.sprite = icon;
            PlacedSaves?.Invoke(new Vector3(positionBuild.x,positionBuild.y,0f), this);
        }

        public void Place(Vector3 position, Transform parent)
        {
            _isPlaced = true;
            _collider.enabled = false;
            transform.position = position;
            transform.parent = parent;
            Finished?.Invoke(this);
        }

        public void Delete()
        {
            _collider.enabled = true;
            transform.position = Vector3.zero;
            transform.parent = null;
            _isActive = false;
            _isPlaced = false;
            gameObject.SetActive(false);
        }
    }
}