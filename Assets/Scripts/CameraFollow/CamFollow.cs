using UnityEngine;

namespace CameraFollow
{
    public class CamFollow: MonoBehaviour
    {

        public void Init(Vector2Int gridSize)
        {
            transform.position = new Vector3(gridSize.x / 2f, gridSize.y / 2f, -gridSize.x);
        }
    }
}