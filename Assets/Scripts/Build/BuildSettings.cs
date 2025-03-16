using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

[Serializable]
public class BuildSettings
{
    [SerializeField] private List<BuildConfig> _buildConfigs;

    public List<BuildConfig> BuildConfigs => _buildConfigs;
}