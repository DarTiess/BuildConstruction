using System;
using Build;
using DG.Tweening;
using UnityEngine;

namespace Card
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Ground : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private GroundConfig _config;
        private bool _isOccupied;
        private Vector2Int _gridPosition;
        private Building _building;

        public bool IsOccupied => _isOccupied;

        public event Action<Vector2Int, BuildType> BuildFinished;
        public event Action<Vector2Int> RemoveBuild;

        public void Initialize(GroundConfig config, Vector2Int gridPosition)
        {
            _config = config;
            _gridPosition = gridPosition;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _config.Icon;
            _isOccupied = false;
        }

        public void SetOccupied(Building building)
        {
            _isOccupied = true;
            _building = building;
            _spriteRenderer.DOColor(Color.white, 0.5f).From(Color.green);
            BuildFinished?.Invoke(_gridPosition, building.Type);
        }

        public void ShowOccupied()
        {
            _spriteRenderer.DOColor(Color.white, 0.5f).From(Color.red);
        }

        public void FreeGround()
        {
            _isOccupied = false;
            _spriteRenderer.DOColor(Color.white, 0.5f).From(Color.green);
            RemoveBuild?.Invoke(_gridPosition);
            _building.Delete();
        }

        public void SetSaved(Building objBuild)
        {
            _isOccupied = true;
            _building = objBuild;
            _spriteRenderer.DOColor(Color.white, 0.5f).From(Color.green);
        }
    }

   
}