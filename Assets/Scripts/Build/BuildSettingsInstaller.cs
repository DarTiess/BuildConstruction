using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "BuildSettingsInstaller", menuName = "Installers/BuildSettingsInstaller")]
public class BuildSettingsInstaller : ScriptableObjectInstaller<BuildSettingsInstaller>
{
    public BuildSettings BuildSettings;
    public override void InstallBindings()
    {
        Container.BindInstance(BuildSettings);
    }
}