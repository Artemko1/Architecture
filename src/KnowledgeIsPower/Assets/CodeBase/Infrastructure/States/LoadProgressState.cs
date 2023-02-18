using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService,
            ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgress();
            EnterLoadLevel();
        }

        public void Exit()
        {
        }

        private void LoadProgress() =>
            _progressService.Progress = _saveLoadService.LoadProgress();

        private void EnterLoadLevel() =>
            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.PlayerState.PositionOnLevel.LevelName);

        public class Factory : PlaceholderFactory<LoadProgressState>
        {
        }
    }
}