using UnityEngine;

namespace Grid
{
    public interface IGridInit
    {
        public void Init(Vector2Int gridSize ,float cellSize ,float cellSpacing);
    }
}