using System;
using System.Collections.Generic;
using Data;
using Messeges;
using Messenger;
using SaveService;
using UnityEngine;

namespace BuildingObjects
{
    public class BuildFactory: IDisposable, ILoader, IBuildFactory
    {
        private ObjectPoole<Building> _poole;
        private Camera _camera;
        private List<Building> _buildList=new();
        private readonly BuildSettings _buildSettings;
        private readonly IMessenger _messenger;
        private readonly ILoadHandler _persistentData;

        public BuildFactory(BuildSettings buildSettings, IMessenger messenger, ILoadHandler persistentData)
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
            _camera = camera;

           _persistentData.LoadToObject(this);
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

        public void Dispose()
        {
            _messenger.Unsub<SelectBuild>(OnSelectedBuild);
            _buildList.ForEach(x=>x.Placed-=OnPlacedBuild);
            _buildList.ForEach(x=>x.Finished-=OnFinishedBuild);
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

        private void OnPlacedBuildSaves(Vector3 pos, Building build)
        {
            build.PlacedSaves -= OnPlacedBuildSaves;
            _messenger.Pub(new PlacedSaved{Position=pos,Build = build});
        }
    }
}