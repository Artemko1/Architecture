using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticDataProvider;
using CodeBase.StaticData.Hero;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStaticDataProviderService _staticDataProviderService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService,
            ISaveLoadService saveLoadService, IStaticDataProviderService staticDataProviderService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _staticDataProviderService = staticDataProviderService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.PlayerState.PositionOnLevel.LevelName);
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew() =>
            _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();

        private PlayerProgress NewProgress()
        {
            PlayerProgressStaticData progressStaticData = _staticDataProviderService.ForNewGame();

            var positionOnLevel =
                new PositionOnLevel(progressStaticData.PositionOnLevel.LevelName, progressStaticData.PositionOnLevel.Position);

            HeroStaticData heroStaticData = _staticDataProviderService.ForHero();

            var playerState = new PlayerState(positionOnLevel, heroStaticData.Stats.HealthData.MaxHp);
            var progress = new PlayerProgress(playerState);

            return progress;
        }
    }
}