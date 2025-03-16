using System;
using System.Collections.Generic;
using Build;
using Card;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildWindow: MonoBehaviour, IDisposable
    {
        [SerializeField] private Button _setButton;
        [SerializeField] private Button _deleteButton;
        [SerializeField] private BuildView _buildViewPrefab;
        [SerializeField] private Transform _container;
        private List<BuildView> _buildViewList=new ();
        private List<BuildConfig> _buildConfigs;
        private BuildType _buildType;
        private bool _onDelete;

        public event Action<BuildType> OnSelectedBuild;
        public event Action OnDeleteBuild;

        public void Init(List<BuildConfig> buildConfigs)
        {
            _buildConfigs = buildConfigs;
            foreach (BuildConfig buildConfig in _buildConfigs)
            {
                var build = Instantiate(_buildViewPrefab, _container);
                build.Init(buildConfig);
                build.Clicked += SelectBuild;
                _buildViewList.Add(build);
            }
        }

        private void OnEnable()
        {
            _setButton.onClick.AddListener(InstallBuild);
            _deleteButton.onClick.AddListener(DeleteBuild);
        }

        private void OnDisable()
        {
            _setButton.onClick.RemoveListener(InstallBuild);
            _deleteButton.onClick.RemoveListener(DeleteBuild);
        }

        private void SelectBuild(BuildType buildType)
        {
           _buildType = buildType;
        }

        private void DeleteBuild()
        {
           OnDeleteBuild?.Invoke();
            _buildViewList.ForEach(x=>x.UnClicked());
        }
        private void InstallBuild()
        {
            _buildViewList.ForEach(x=>x.UnClicked());
            OnSelectedBuild?.Invoke(_buildType);
        }

        public void Dispose()
        {
            _buildViewList.ForEach(x=>x.Clicked-=SelectBuild);
        }
    }
}