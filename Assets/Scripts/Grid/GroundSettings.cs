using System;
using Card;
using UnityEngine;

[Serializable]
public class GroundSettings
{
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(10, 10);
    [SerializeField] private float _cellSize = 1f;
    [SerializeField] private float _cellSpacing = 0.1f;

    [SerializeField] private Ground _groundPrefab; 
    [SerializeField] private GroundConfig[] _groundConfig;

    public Vector2Int GridSize => _gridSize;
    public float CellSize => _cellSize;
    public float CellSpacing => _cellSpacing;
    public Ground GroundPrefab => _groundPrefab;
    public GroundConfig[] GroundConfig => _groundConfig;
}