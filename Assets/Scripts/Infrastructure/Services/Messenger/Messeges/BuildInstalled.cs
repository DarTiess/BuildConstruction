using Card;
using UnityEngine;

namespace Infrastructure.Services.Messeges
{
    public struct BuildInstalled
    {
        private readonly Vector2Int _gridPosition;
        private readonly BuildType _type;

        public Vector2Int GridPosition => _gridPosition;
        public BuildType Type => _type;

        public BuildInstalled(Vector2Int gridPosition, BuildType type)
        {
            _gridPosition = gridPosition;
            _type = type;
        }
    }
}