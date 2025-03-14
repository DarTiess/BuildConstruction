using System;
using Card;
using UnityEngine;

namespace UI
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