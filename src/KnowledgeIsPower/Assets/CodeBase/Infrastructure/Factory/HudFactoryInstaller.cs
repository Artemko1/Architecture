using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class HudFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings() =>
            Container.Bind<HudFactory>().AsSingle();
    }
}