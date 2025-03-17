using Data;
using UnityEngine;

namespace BuildingObjects
{
    public interface IBuildFactory
    {
        void Init(Building prefab, int size, Transform parent, Camera camera);
    }
}