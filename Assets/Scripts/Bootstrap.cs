using System;
using Build;
using CameraFollow;
using CodeBase.Infrastructure;
using Grid;
using Infrastructure.Services;
using UI;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class Bootstrap: MonoBehaviour
    {
        [SerializeField] private Transform _canvas;
        [SerializeField] private Transform _gridOrigin;
        [SerializeField] private Building _buildPrefab;
        [SerializeField] private int _poolSize;
        [SerializeField] private Transform _buildContainer;
        private GroundFactory _groundFactory;
        private UIRoot _uiRoot;
        private MediatorFactory _mediatorFactory;
        private Messenger _messenger;
        private CamFollow _camera;
        private GroundSettings _groundSettings;
        private BuildFactory _buildFactory;
        private PersistentData _persistentData;

        [Inject]
        public void Construct(MediatorFactory mediatorFactory, Messenger messenger,
            GroundSettings groundSettings, BuildFactory buildFactory, GroundFactory groundFactory,
            PersistentData persistentData)
        {
            _mediatorFactory = mediatorFactory;
            _messenger = messenger;
            _groundSettings = groundSettings;
            _buildFactory = buildFactory;
            _groundFactory = groundFactory;
            _persistentData = persistentData;
        }
        private void Awake()
        {
            InitPersistenData();
            CreateUiRoot();
            SetCameraPosition();
            CreateGroundFactory();
            InitBuildFactory();
        }

        private void InitPersistenData()
        {
            _persistentData.Initialize();
        }

        private void SetCameraPosition()
        {
            _camera = Camera.main.GetComponent<CamFollow>();
            _camera.Init(_groundSettings.GridSize);
        }

        private void CreateGroundFactory()
        {
            _groundFactory.Init(_gridOrigin);
        }

        private void CreateUiRoot()
        {
            _mediatorFactory.Init(_canvas, _messenger);
            _uiRoot = new UIRoot(_mediatorFactory);
            _uiRoot.Init();
        }

        private void InitBuildFactory()
        {
            _buildFactory.Init(_buildPrefab, _poolSize, _buildContainer, Camera.main);
        }
    }
}