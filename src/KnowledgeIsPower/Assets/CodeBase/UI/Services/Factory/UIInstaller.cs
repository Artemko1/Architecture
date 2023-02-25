using CodeBase.UI.Services.Windows;
using Zenject;

namespace CodeBase.UI.Services.Factory
{
    public class UIInstaller : MonoInstaller<UIInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle();
            Container.Bind<WindowService>().AsSingle();
        }
    }
}