using System;
using UnityEngine;

namespace CameraFollow
{
    public class CamFollow: MonoBehaviour
    {
       [SerializeField] private Camera _camera;
       [SerializeField] private float _offset;
       
        public void Init(Vector2Int gridSize)
        {
             var zPosition = gridSize.x > gridSize.y ? -gridSize.x : -gridSize.y;
            transform.position = new Vector3(gridSize.x / 2f, gridSize.y / 2f, zPosition);
       
            float aspectRatio = (float)Screen.width / Screen.height;
           _camera.orthographicSize = Mathf.Max(gridSize.y / 2f, (gridSize.x / aspectRatio) / 2f)+ _offset;
        }
        
    }
}