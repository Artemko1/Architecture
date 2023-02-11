using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticDataProvider;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";

        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        private readonly GameStateMachine _stateMachine;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        public void Enter() =>
            _sceneLoader.Load(Initial, EnterLoadProgress);

        public void Exit()
        {
        }

        private void RegisterServices()
        {
            RegisterInputService();

            IStaticDataProviderService staticDataService = RegisterStaticData();

            _services.RegisterSingle<IGameStateMachine>(_stateMachine);

            IAssetProviderService assetProviderService = new AssetProviderService();
            _services.RegisterSingle(assetProviderService);

            IPersistentProgressService persistentProgressService = new PersistentProgressService();
            _services.RegisterSingle(persistentProgressService);

            ISaveLoadService saveLoadService = new SaveLoadService(persistentProgressService, staticDataService);
            _services.RegisterSingle(saveLoadService);

            IUIFactory uiFactory = new UIFactory(assetProviderService, staticDataService, persistentProgressService);
            _services.RegisterSingle(uiFactory);

            IWindowService windowService = new WindowService(uiFactory);
            _services.RegisterSingle(windowService);

            IRandomService randomService = new RandomService();
            _services.RegisterSingle(randomService);

            IGameFactory factory = new GameFactory(assetProviderService, staticDataService, randomService, persistentProgressService,
                saveLoadService, windowService);
            _services.RegisterSingle(factory);
        }

        private void RegisterInputService()
        {
            IInputService inputService = Application.isEditor
                ? new StandaloneInputService()
                : new MobileInputService();
            _services.RegisterSingle(inputService);
        }

        private IStaticDataProviderService RegisterStaticData()
        {
            IStaticDataProviderService staticData = new StaticDataProviderService();
            staticData.Load();
            _services.RegisterSingle(staticData);
            return staticData;
        }

        private void EnterLoadProgress() =>
            _stateMachine.Enter<LoadProgressState>();
    }
}