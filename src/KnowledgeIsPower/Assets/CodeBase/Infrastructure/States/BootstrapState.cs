﻿using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.StaticData;
using CodeBase.Services.Input;
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
            RegisterStaticData();
            _services.RegisterSingle(InputService());
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>(),
                _services.Single<IStaticDataService>()));
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(),
                _services.Single<IGameFactory>()));
        }

        private void RegisterStaticData()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.LoadMonsters();
            _services.RegisterSingle(staticData);
        }

        private static IInputService InputService() =>
            Application.isEditor ? new StandaloneInputService() : new MobileInputService();

        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadProgressState>();
    }
}