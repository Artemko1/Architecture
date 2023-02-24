using Zenject;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactoryInstaller : MonoInstaller<UIFactoryInstaller>
    {
        public override void InstallBindings() =>
            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle();
    }
}