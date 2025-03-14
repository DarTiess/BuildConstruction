using System;
using CameraFollow;
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
        [SerializeField] private GridBuilder _cardGridBuilder;
        private GroundFactory _groundRandomFactory;
        private UIRoot _uiRoot;
        private MediatorFactory _mediatorFactory;
        private Messenger _messenger;
        private CamFollow _camera;
        private GroundSettings _groundSettings;

        [Inject]
        public void Construct(MediatorFactory mediatorFactory, Messenger messenger, GroundSettings groundSettings)
        {
            _mediatorFactory = mediatorFactory;
            _messenger = messenger;
            _groundSettings = groundSettings;
        }
        private void Awake()
        {
            CreateCardSpawner();
            RegisterMessenger();
            CreateUiRoot();
            SetCameraPosition();
        }

        private void SetCameraPosition()
        {
            _camera = Camera.main.GetComponent<CamFollow>();
            _camera.Init(_groundSettings.GridSize);
        }

        private void CreateCardSpawner()
        {
            _groundRandomFactory = new GroundFactory(_cardGridBuilder);
        }

        private void RegisterMessenger()
        {
           // _serviceLocator.Reg(new Messenger());
        }

        private void CreateUiRoot()
        {
            _mediatorFactory.Init(_canvas, _messenger);
            _uiRoot = new UIRoot(_mediatorFactory);
            _uiRoot.Init();
        }
    }
}