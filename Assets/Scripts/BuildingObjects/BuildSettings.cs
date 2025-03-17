using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace BuildingObjects
{
    [Serializable]
    public class BuildSettings
    {
        [SerializeField] private List<BuildConfig> _buildConfigs;

        public List<BuildConfig> BuildConfigs => _buildConfigs;
    }
}