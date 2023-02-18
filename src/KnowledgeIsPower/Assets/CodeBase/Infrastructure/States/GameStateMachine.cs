using System;
using System.Collections.Generic;
using CodeBase.Services;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine : IService
    {
        private readonly BootstrapState.Factory _bootstrapStateFactory;
        private readonly GameLoopState.Factory _gameLoopStateFactory;
        private readonly LoadLevelState.Factory _loadLevelStateFactory;
        private readonly LoadProgressState.Factory _loadProgressStateFactory;

        private readonly Dictionary<Type, IExitableState> _states = new Dictionary<Type, IExitableState>();

        private IExitableState _activeState;

        [Inject]
        public GameStateMachine(BootstrapState.Factory bootstrapStateFactory,
            LoadLevelState.Factory loadLevelStateFactory,
            LoadProgressState.Factory loadProgressStateFactory,
            GameLoopState.Factory gameLoopStateFactory)
        {
            _bootstrapStateFactory = bootstrapStateFactory;
            _loadLevelStateFactory = loadLevelStateFactory;
            _loadProgressStateFactory = loadProgressStateFactory;
            _gameLoopStateFactory = gameLoopStateFactory;
        }

        public void Startup()
        {
            _states.Add(typeof(BootstrapState), _bootstrapStateFactory.Create());
            _states.Add(typeof(LoadLevelState), _loadLevelStateFactory.Create());
            _states.Add(typeof(LoadProgressState), _loadProgressStateFactory.Create());
            _states.Add(typeof(GameLoopState), _gameLoopStateFactory.Create());

            Enter<BootstrapState>();
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