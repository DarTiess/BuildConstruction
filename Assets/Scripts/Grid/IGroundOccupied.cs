using System;
using Card;

namespace Grid
{
    public interface IGroundOccupied
    {
        event Action<int, BuildType> IsOccupied;
    }
}