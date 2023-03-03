using System.Threading.Tasks;
using CodeBase.Services.StaticDataProvider;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly StaticDataProviderService _staticDataProviderService;

        [Inject]
        public BootstrapState(GameStateMachine stateMachine, StaticDataProviderService staticDataProviderService)
        {
            _stateMachine = stateMachine;
            _staticDataProviderService = staticDataProviderService;
        }

        public async void Enter()
        {
            SetTargetFramerate();

            await InitializeServices();

            EnterLoadProgress();
        }

        public void Exit()
        {
        }

        private async Task InitializeServices()
        {
            await Addressables.InitializeAsync().Task;
            await _staticDataProviderService.Load();
        }

        private static void SetTargetFramerate() =>
            Application.targetFrameRate = 60;

        private void EnterLoadProgress() =>
            _stateMachine.Enter<LoadProgressState>();

        public class Factory : PlaceholderFactory<BootstrapState>
        {
        }
    }
}