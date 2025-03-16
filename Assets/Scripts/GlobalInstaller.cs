using Build;
using CodeBase.Infrastructure;
using Grid;
using Infrastructure.Services;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ServiceLocator>().AsSingle();
        Container.BindInterfacesAndSelfTo<Messenger>().AsSingle();
        Container.BindInterfacesAndSelfTo<GroundFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<MediatorFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<BuildFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<PersistentData>().AsSingle();
    }
}