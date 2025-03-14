using System;
using Grid;
using Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class Bootstrap: MonoBehaviour
    {
        [SerializeField] private GridBuilder _cardGridBuilder;
        private GroundSpawner _groundRandomSpawner;
        private IServiceLocator _serviceLocator;

        [Inject]
        public void Construct(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }
        private void Awake()
        {
            CreateCardSpawner();
            RegisterMessenger();
        }

        private void RegisterMessenger()
        {
            _serviceLocator.Reg(new Messenger());
        }

        private void CreateCardSpawner()
        {
            _groundRandomSpawner = new GroundSpawner(_cardGridBuilder);
        }
    }
}