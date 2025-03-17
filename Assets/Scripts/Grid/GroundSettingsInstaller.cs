using UnityEngine;
using Zenject;

namespace Grid
{
    [CreateAssetMenu(fileName = "GroundSettingsInstaller", menuName = "Installers/GroundSettingsInstaller")]
    public class GroundSettingsInstaller : ScriptableObjectInstaller<GroundSettingsInstaller>
    {
        public GroundSettings GroundSettings;
        public override void InstallBindings()
        {
            Container.BindInstance(GroundSettings);
        }
    }
}