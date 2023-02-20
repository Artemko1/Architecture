using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticDataProvider;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtain _loadingCurtainPrefab;
        [SerializeField] private GameObject _eventSystem;

        public override void InstallBindings()
        {
            Debug.Log("Installing Project bindings");

            InstallCoroutineRunner();
            InstallEventSystem();
            InstallCurtain();

            BindGame();
            BindSceneLoader();
            BindServices();
        }

        private void InstallEventSystem() =>
            Container.InstantiatePrefab(_eventSystem);

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

        private void BindSceneLoader() =>
            Container.Bind<SceneLoader>().AsSingle();

        private void BindServices()
        {
            IInputService inputService = Application.isEditor
                ? new StandaloneInputService()
                : new MobileInputService();
            Container.Bind<IInputService>().FromInstance(inputService).AsSingle();

            Container.Bind<AssetProviderService>().AsSingle();
            Container.Bind<IStaticDataProviderService>().To<StaticDataProviderService>().AsSingle();
            Container.Bind<PersistentProgressService>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<UIFactory>().AsSingle();
            Container.Bind<WindowService>().AsSingle();
            Container.Bind<RandomService>().AsSingle();
        }
    }
}