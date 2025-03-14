using System;
using Card;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Grid
{
    public class GridBuilder : MonoBehaviour, ICardUpgrade, IGroundOccupied
    {
        [SerializeField] private Transform gridOrigin;
        public Vector2Int GridSize => _gridSize;

        private Ground[,] _gameObjectGrid;
        private Vector2Int _gridSize;
        private float _cellSize;
        private float _cellSpacing;
        private Ground _groundPrefab;
        private GroundConfig[] _configs;

        public event Action CardUpgrade;
        public event Action<int, BuildType> IsOccupied;

        [Inject]
        public void Construct(GroundSettings groundSettings)
        {
            _configs = groundSettings.GroundConfig;
            _gridSize = groundSettings.GridSize;
            _cellSize = groundSettings.CellSize;
            _cellSpacing = groundSettings.CellSpacing;
            _groundPrefab = groundSettings.GroundPrefab;
            _gameObjectGrid = new Ground[_gridSize.x,_gridSize.y];
        }

        public void FillGrid()
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    Ground newcard = SpawnObjectAtGridPosition(_groundPrefab, new Vector2Int(x, y));
                    int rndConfig = Random.Range(0, _configs.Length);
                    newcard.Initialize(_configs[rndConfig]);
                }
            }
        }
    
        public void SpawnObjectAtFreePosition( Vector2Int gridPosition, GroundConfig config)
        {
            var instance = _gameObjectGrid[gridPosition.x, gridPosition.y];
            instance.gameObject.SetActive(true);
            SetObjectAtGridPosition(instance, gridPosition);
            instance.transform.position = GridToWorldPosition(gridPosition);
            instance.Initialize(config);
        }
        public bool HasCardAtGridPosition(Vector2Int gridPosition)
        {
            return _gameObjectGrid[gridPosition.x, gridPosition.y].gameObject.activeInHierarchy;
        }
        public bool HasEmptyGridPositions()
        {
            foreach (var gameObject in _gameObjectGrid)
            {
                if (!gameObject.gameObject.activeInHierarchy)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnDisable()
        {
            foreach (Ground card in _gameObjectGrid)
            {
                card.GroundUpgrade -= OnGroundUpgrade;
                card.CollectCard -= OnCollectedCard;
            }
        }

        private Vector3 GridToWorldPosition(Vector2Int gridPosition)
        {
            var offset = gridOrigin.position;
            return (new Vector3(gridPosition.x * (_cellSize + _cellSpacing), gridPosition.y * (_cellSize + _cellSpacing)) + offset);
        }

        private void SetObjectAtGridPosition(Ground gameObject, Vector2Int gridPosition)
        {
            _gameObjectGrid[gridPosition.x, gridPosition.y] = gameObject;
            gameObject.transform.position = GridToWorldPosition(gridPosition);
        }

        private Ground SpawnObjectAtGridPosition(Ground gameObjectPrefab, Vector2Int gridPosition)
        {
            var ground =Instantiate(gameObjectPrefab, gridOrigin);
            ground.GroundUpgrade += OnGroundUpgrade;
            ground.CollectCard += OnCollectedCard;
            SetObjectAtGridPosition(ground, gridPosition);
            ground.transform.position = GridToWorldPosition(gridPosition);
            return ground;
        }

        private void OnCollectedCard(int price, BuildType type)
        {
            IsOccupied?.Invoke(price, type);
        }

        private void OnGroundUpgrade()
        {
            CardUpgrade?.Invoke();
        }
    }
}