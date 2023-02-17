using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtain _loadingCurtainPrefab;

        public override void InstallBindings()
        {
            Debug.Log("Installing Project bindings");
            var loadingCurtain = Container.InstantiatePrefabForComponent<LoadingCurtain>(_loadingCurtainPrefab);
            Container.Bind<LoadingCurtain>().FromInstance(loadingCurtain).AsSingle();

            Container.Bind<ICoroutineRunner>()
                .To<CoroutineRunner>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("PersistentCoroutineRunner")
                .AsSingle();
        }
    }
}