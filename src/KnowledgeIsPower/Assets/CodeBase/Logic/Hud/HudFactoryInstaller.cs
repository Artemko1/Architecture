using Zenject;

namespace CodeBase.Logic.Hud
{
    public class HudFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings() =>
            Container.Bind<HudFactory>().AsSingle();
    }
}