using Zenject;

namespace CodeBase.Infrastructure
{
    public class SceneInitializerInstaller : MonoInstaller<SceneInitializerInstaller>
    {
        public override void InstallBindings() =>
            Container.BindInterfacesTo<SceneInitializer>().AsSingle();
    }
}