using BuildingObjects;
using Grid;
using SaveService;
using UI;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Messenger.Messenger>().AsSingle();
        Container.BindInterfacesAndSelfTo<GroundFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<MediatorFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<BuildFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<PersistentData>().AsSingle();
    }
}