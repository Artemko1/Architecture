using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.AssetProvider;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.Randomizer;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
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
            _sceneLoader.Load(Initial, EnterLoadLevel);

        public void Exit()
        {
        }

        private void RegisterServices()
        {
            RegisterInputService();

            IStaticDataService staticDataService = RegisterStaticData();

            IAssetProviderService assetProviderService = new AssetProviderService();
            _services.RegisterSingle(assetProviderService);

            IPersistentProgressService persistentProgressService = new PersistentProgressService();
            _services.RegisterSingle(persistentProgressService);

            IRandomService randomService = new RandomService();
            _services.RegisterSingle(randomService);

            IGameFactory factory = new GameFactory(assetProviderService, staticDataService, randomService, persistentProgressService);
            _services.RegisterSingle(factory);

            ISaveLoadService saveLoadService = new SaveLoadService(persistentProgressService, factory);
            _services.RegisterSingle(saveLoadService);
        }

        private void RegisterInputService()
        {
            IInputService inputService = Application.isEditor
                ? new StandaloneInputService()
                : new MobileInputService();
            _services.RegisterSingle(inputService);
        }

        private IStaticDataService RegisterStaticData()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.LoadMonsters();
            _services.RegisterSingle(staticData);
            return staticData;
        }

        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadProgressState>();
    }
}