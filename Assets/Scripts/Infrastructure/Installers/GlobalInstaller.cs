using Infrastructure.Services;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ServiceLocator>().AsSingle();
        Container.BindInterfacesAndSelfTo<Messenger>().AsSingle();
        Container.BindInterfacesAndSelfTo<MediatorFactory>().AsSingle();
    }
}