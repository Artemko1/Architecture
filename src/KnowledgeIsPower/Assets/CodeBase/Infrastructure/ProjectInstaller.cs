using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticDataProvider;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtain _loadingCurtainPrefab;

        public override void InstallBindings()
        {
            InstallCoroutineRunner();
            InstallCurtain();

            BindGame();
            BindServices();
        }

        private void InstallCoroutineRunner() =>
            Container.Bind<ICoroutineRunner>()
                .To<CoroutineRunner>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("PersistentCoroutineRunner")
                .AsSingle();

        private void InstallCurtain() =>
            Container.Bind<LoadingCurtain>().FromComponentInNewPrefab(_loadingCurtainPrefab).AsSingle();

        private void BindGame()
        {
            Container.Bind<Game>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();

            Container.BindFactory<BootstrapState, BootstrapState.Factory>().AsSingle();
            Container.BindFactory<LoadLevelState, LoadLevelState.Factory>().AsSingle();
            Container.BindFactory<LoadProgressState, LoadProgressState.Factory>().AsSingle();
            Container.BindFactory<GameLoopState, GameLoopState.Factory>().AsSingle();
        }


        private void BindServices()
        {
            IInputService inputService = Application.isEditor
                ? new StandaloneInputService()
                : new MobileInputService();
            Container.Bind<IInputService>().FromInstance(inputService).AsSingle();

            Container.Bind<SceneLoader>().AsSingle();

            Container.Bind<AssetProviderService>().AsSingle();
            Container.Bind<StaticDataProviderService>().AsSingle();
            Container.Bind<PersistentProgressService>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<RandomService>().AsSingle();
        }
    }
}