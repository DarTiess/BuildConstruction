using System;
using Card;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildView: MonoBehaviour
    {
        [SerializeField] private Image _buildIcon;
        [SerializeField] private Button _buildButton;
        [SerializeField] private Image _backImage;
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _inactiveSprite;
        private BuildType _type;

        public event Action<BuildType> Clicked;

        public void Init(BuildConfig buildConfig)
        {
            _buildIcon.sprite = buildConfig.Icon;
            _type = buildConfig.Type;
        }

        private void OnEnable()
        {
            _buildButton.onClick.AddListener(ClickBuildView);
        }

        private void OnDisable()
        {
            _buildButton.onClick.RemoveListener(ClickBuildView);
        }

        private void ClickBuildView()
        {
            _backImage.sprite = _activeSprite;
            Clicked?.Invoke(_type);
        }

        public void UnClicked()
        {
            _backImage.sprite = _inactiveSprite;
        }
    }
}