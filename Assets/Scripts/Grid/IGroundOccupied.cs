using System;
using Card;
using UnityEngine;

namespace Grid
{
    public interface IGroundOccupied
    {
        event Action<Vector2Int, BuildType> IsOccupied;
    }
}