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
        private readonly IStaticDataProviderService _staticDataProviderService;

        public BootstrapState(GameStateMachine stateMachine, IStaticDataProviderService staticDataProviderService)
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
            _staticDataProviderService.Load();
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