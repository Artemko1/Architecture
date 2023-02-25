using Zenject;

namespace CodeBase.Logic.Hero.Factory
{
    public class HeroFactoryInstaller : MonoInstaller<HeroFactoryInstaller>
    {
        public override void InstallBindings() =>
            Container.Bind<HeroFactory>().AsSingle();
    }
}