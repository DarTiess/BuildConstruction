﻿using System;
using Data;
using Grid.Ground;
using Messeges;
using Messenger;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Grid
{
    [Serializable]
    public class GroundFactory: IDisposable, IGroundFactory
    {
        private Transform _gridOrigin;
        private Ground.Ground[,] _gameObjectGrid;
        private Vector2Int _gridSize;
        private float _cellSize;
        private float _cellSpacing;
        private Ground.Ground _groundPrefab;
        private GroundConfig[] _configs;
        private readonly IMessenger _messenger;

        public GroundFactory(GroundSettings groundSettings, IMessenger messenger)
        {
            _messenger = messenger;
            _configs = groundSettings.GroundConfig;
            _gridSize = groundSettings.GridSize;
            _cellSize = groundSettings.CellSize;
            _cellSpacing = groundSettings.CellSpacing;
            _groundPrefab = groundSettings.GroundPrefab;
            _gameObjectGrid = new Ground.Ground[_gridSize.x,_gridSize.y];
            _messenger.Sub<CheckGroundForBuilding>(OnCheckGroundForBuilding);
            _messenger.Sub<PlacedSaved>(OnPlacedSavedBuilding);
        }

        public void Init(Transform gridOrigin)
        {
            _gridOrigin = gridOrigin;
            FillGrid();
        }

        private void OnPlacedSavedBuilding(PlacedSaved obj)
        {
            Vector2Int gridPos = WorldToGridPosition(obj.Position);
            if (!IsValidGridPosition(gridPos)) 
                return;

            Ground.Ground ground = _gameObjectGrid[gridPos.x, gridPos.y];
           
                ground.SetSaved(obj.Build);
                obj.Build.Place(ground.transform.position, ground.transform);
            
        }


        private void OnCheckGroundForBuilding(CheckGroundForBuilding obj)
        {
            Vector2Int gridPos = WorldToGridPosition(obj.Position);
            if (!IsValidGridPosition(gridPos)) 
                return;

            Ground.Ground ground = _gameObjectGrid[gridPos.x, gridPos.y];
            if (!ground.IsOccupied)
            {
                ground.SetOccupied(obj.Build);
                obj.Build.Place(ground.transform.position, ground.transform);
            }
            else
            {
                ground.ShowOccupied();
            }
        }

        private bool IsValidGridPosition(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < _gridSize.x && pos.y >= 0 && pos.y < _gridSize.y;
        }

        private Vector2Int WorldToGridPosition(Vector3 worldPos)
        {
            Vector3 localPos = worldPos - _gridOrigin.position;
            return new Vector2Int(
                Mathf.RoundToInt(localPos.x / (_cellSize + _cellSpacing)),
                Mathf.RoundToInt(localPos.y / (_cellSize + _cellSpacing))
            );
        }

        private void FillGrid()
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    Ground.Ground ground = SpawnObjectAtGridPosition(_groundPrefab, new Vector2Int(x, y));
                    int rndConfig = Random.Range(0, _configs.Length);
                    ground.Initialize(_configs[rndConfig], new Vector2Int(x, y));
                }
            }
        }

        private Ground.Ground SpawnObjectAtGridPosition(Ground.Ground gameObjectPrefab, Vector2Int gridPosition)
        {
            var ground =Object.Instantiate(gameObjectPrefab, _gridOrigin);
            ground.BuildFinished += InstallBuilding;
            ground.RemoveBuild += RemoveBuilding;
            SetObjectAtGridPosition(ground, gridPosition);
            ground.transform.position = GridToWorldPosition(gridPosition);
            return ground;
        }

        private void RemoveBuilding(Vector2Int position)
        {
            _messenger.Pub(new DeleteBuild {Position=position  });
        }

        private void InstallBuilding(Vector2Int gridPosition, BuildType type)
        {
            _messenger.Pub(new BuildInstalled(gridPosition, type));
        }

        private void SetObjectAtGridPosition(Ground.Ground gameObject, Vector2Int gridPosition)
        {
            _gameObjectGrid[gridPosition.x, gridPosition.y] = gameObject;
            gameObject.transform.position = GridToWorldPosition(gridPosition);
        }

        private Vector3 GridToWorldPosition(Vector2Int gridPosition)
        {
            var offset = _gridOrigin.position;
            return (new Vector3(gridPosition.x * (_cellSize + _cellSpacing), gridPosition.y * (_cellSize + _cellSpacing)) + offset);
        }
        
        public void Dispose()
        {
            _messenger.Unsub<CheckGroundForBuilding>(OnCheckGroundForBuilding);
        }
    }
}