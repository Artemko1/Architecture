using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticDataProvider;
using CodeBase.UI.Services.Factory;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain curtain, AllServices services)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                {
                    typeof(BootstrapState),
                    new BootstrapState(this, services)
                },
                {
                    typeof(LoadLevelState),
                    new LoadLevelState(this,
                        sceneLoader,
                        curtain,
                        services.Single<IGameFactory>(),
                        services.Single<IStaticDataProviderService>(),
                        services.Single<IUIFactory>(),
                        services.Single<IAssetProviderService>())
                },
                {
                    typeof(LoadProgressState),
                    new LoadProgressState(this,
                        services.Single<IPersistentProgressService>(),
                        services.Single<ISaveLoadService>())
                },
                {
                    typeof(GameLoopState),
                    new GameLoopState(this)
                },
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            var state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}