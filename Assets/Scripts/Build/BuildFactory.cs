using System;
using System.Collections.Generic;
using Infrastructure;
using Infrastructure.Services;
using Infrastructure.Services.Messeges;
using UI;
using UnityEngine;

namespace Build
{
    public class BuildFactory: IDisposable
    {
        private ObjectPoole<Building> _poole;
        private readonly Messenger _messenger;
        private Camera _camera;
        private readonly BuildSettings _buildSettings;
        private List<Building> _buildList=new();

        public BuildFactory(BuildSettings buildSettings, Messenger messenger)
        {
            _buildSettings = buildSettings;
            _messenger = messenger;
            _poole = new ObjectPoole<Building>();
        }

        public void Init(Building prefab, int size, Transform parent, Camera camera)
        {
            _poole.CreatePool(prefab, size, parent);
            _messenger.Sub<SelectBuild>(OnSelectedBuild);
            _messenger.Sub<DeleteBuild>(OnTryDeleteBuild);
            _camera = camera;
        }

        private void OnTryDeleteBuild(DeleteBuild obj)
        {
            
        }

        private void OnSelectedBuild(SelectBuild obj)
        {
            Debug.Log("Selected "+obj.BuildType+" build");

            var build = _poole.GetObject();
            _buildList.Add(build);
            build.Placed += OnPlacedBuild;
            build.SetType(obj.BuildType, 
                _buildSettings.BuildConfigs.Find(x=>x.Type==obj.BuildType).Icon,
                _camera);
        }

        private void OnPlacedBuild(Vector3 position, Building build)
        {
            Debug.Log("Checj Ground");
            build.Placed -= OnPlacedBuild;
            _messenger.Pub(new CheckGroundForBuilding{Position=position,Build = build});
            
        }

        public void Dispose()
        {
            _messenger.Unsub<SelectBuild>(OnSelectedBuild);
            _messenger.Unsub<DeleteBuild>(OnTryDeleteBuild);
            _buildList.ForEach(x=>x.Placed-=OnPlacedBuild);
        }
    }
}