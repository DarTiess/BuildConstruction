using System;
using System.Collections.Generic;
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

        public event Action<BuildType> OnSelectedBuild;

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
            OnSelectedBuild?.Invoke(buildType);
        }

        private void DeleteBuild()
        {
            
        }

        private void InstallBuild()
        {
            
        }

        public void Dispose()
        {
            _buildViewList.ForEach(x=>x.Clicked-=SelectBuild);
        }
    }
}