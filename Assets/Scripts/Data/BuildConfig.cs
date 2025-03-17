using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class BuildConfig
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private BuildType _type;

        public Sprite Icon => _icon;
        public BuildType Type => _type;
    }
}