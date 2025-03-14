using System;
using Card;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Grid
{
    [Serializable]
    public class GroundSpawner: IGroundSpawner
    {
        private GridBuilder _groundBuilder;
        
        public GroundSpawner(GridBuilder groundBuilder)
        {
            _groundBuilder = groundBuilder;
            _groundBuilder.FillGrid();
           
        }
        public void SpawnCardOnRandomPosition(GroundConfig config)
        {
            if (!_groundBuilder.HasEmptyGridPositions())
            {
                Debug.LogError("No empty grid positions!");
                return;
            }

            var gridSize = _groundBuilder.GridSize;

            var maxX = gridSize.x;
            var maxY = gridSize.y;
            
            Vector2Int randomGridPosition;
            
            do
            {
                var randomX = Random.Range(0, maxX);
                var randomY = Random.Range(0, maxY);
                
                randomGridPosition = new Vector2Int(randomX, randomY);
            } while (_groundBuilder.HasCardAtGridPosition(randomGridPosition));
            
            
            _groundBuilder.SpawnObjectAtFreePosition(randomGridPosition, config);
        }

       
    }
}