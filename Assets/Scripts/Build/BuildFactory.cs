using System;
using System.Collections.Generic;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.SaveService;
using Infrastructure;
using Infrastructure.Services;
using Infrastructure.Services.Messeges;
using UI;
using UnityEngine;

namespace Build
{
    public class BuildFactory: IDisposable, ILoader
    {
        private ObjectPoole<Building> _poole;
        private readonly Messenger _messenger;
        private Camera _camera;
        private readonly BuildSettings _buildSettings;
        private List<Building> _buildList=new();
        private readonly PersistentData _persistentData;

        public BuildFactory(BuildSettings buildSettings, Messenger messenger, PersistentData persistentData)
        {
            _buildSettings = buildSettings;
            _messenger = messenger;
            _persistentData = persistentData;
            _poole = new ObjectPoole<Building>();
        }

        public void Init(Building prefab, int size, Transform parent, Camera camera)
        {
            _poole.CreatePool(prefab, size, parent);
            _messenger.Sub<SelectBuild>(OnSelectedBuild);
            _messenger.Sub<DeleteBuild>(OnTryDeleteBuild);
            _camera = camera;

           _persistentData.LoadToObject(this);
        }

        private void OnTryDeleteBuild(DeleteBuild obj)
        {
            
        }

        private void OnSelectedBuild(SelectBuild obj)
        {
            var build = _poole.GetObject();
            _buildList.Add(build);
            build.Placed += OnPlacedBuild;
            build.Finished += OnFinishedBuild;
            build.SetType(obj.BuildType, 
                _buildSettings.BuildConfigs.Find(x=>x.Type==obj.BuildType).Icon,
                _camera);
        }

        private void OnFinishedBuild(Building obj)
        {
            obj.Placed -= OnPlacedBuild;
            obj.Finished -= OnFinishedBuild;
        }

        private void OnPlacedBuild(Vector3 position, Building build)
        {
            _messenger.Pub(new CheckGroundForBuilding{Position=position,Build = build});
            
        }

        public void Dispose()
        {
            _messenger.Unsub<SelectBuild>(OnSelectedBuild);
            _messenger.Unsub<DeleteBuild>(OnTryDeleteBuild);
            _buildList.ForEach(x=>x.Placed-=OnPlacedBuild);
            _buildList.ForEach(x=>x.Finished-=OnFinishedBuild);
        }

        public void Load(GameSave save)
        {
            var keys = new List<Vector2Int>(save.BuildMap.Keys); 
            if (keys.Count > 0)
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    var key = keys[i];
                    var buildType = save.BuildMap[key];
            
                    var build = _poole.GetObject();
                    _buildList.Add(build); 
                    build.PlacedSaves += OnPlacedBuildSaves;
                    build.SetSaves(buildType, 
                        _buildSettings.BuildConfigs.Find(x=>x.Type==buildType).Icon,
                        key);
                }
            }
        }

        private void OnPlacedBuildSaves(Vector3 pos, Building build)
        {
            build.PlacedSaves -= OnPlacedBuildSaves;
            _messenger.Pub(new PlacedSaved{Position=pos,Build = build});
        }
    }
}