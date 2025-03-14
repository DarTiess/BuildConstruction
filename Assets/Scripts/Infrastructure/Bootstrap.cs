using System;
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
        private GroundSpawner _groundRandomSpawner;
        private UIRoot _uiRoot;
        private MediatorFactory _mediatorFactory;
        private Messenger _messenger;

        [Inject]
        public void Construct(MediatorFactory mediatorFactory, Messenger messenger)
        {
            _mediatorFactory = mediatorFactory;
            _messenger = messenger;
        }
        private void Awake()
        {
            CreateCardSpawner();
            RegisterMessenger();
            CreateUiRoot();
        }

        private void CreateCardSpawner()
        {
            _groundRandomSpawner = new GroundSpawner(_cardGridBuilder);
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