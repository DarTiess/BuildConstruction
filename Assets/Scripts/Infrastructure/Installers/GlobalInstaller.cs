using Infrastructure.Services;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        CreateServiceLocator();
    }

    private void CreateServiceLocator()
    {
        Container.BindInterfacesAndSelfTo<ServiceLocator>().AsSingle();
    }
}