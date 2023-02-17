using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log($"Start install bindings. Current scene is {SceneManager.GetActiveScene().name}");
            Container.Bind<Game>().AsSingle();
        }
    }
}