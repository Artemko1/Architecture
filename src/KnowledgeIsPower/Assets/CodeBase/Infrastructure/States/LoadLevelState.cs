using CodeBase.Logic;
using CodeBase.Logic.Hero.Factory;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly LoadingCurtain _curtain;
        private readonly HeroFactory _heroFactory;
        private readonly SceneLoader _sceneLoader;

        private readonly GameStateMachine _stateMachine;

        [Inject]
        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() =>
            _curtain.Hide();

        private void OnLoaded() =>
            _stateMachine.Enter<GameLoopState>();

        public class Factory : PlaceholderFactory<LoadLevelState>
        {
        }
    }
}