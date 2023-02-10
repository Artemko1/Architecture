using System;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticDataProvider;
using CodeBase.StaticData.Hero;
using UnityEngine;

namespace CodeBase.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataProviderService _staticData;

        public SaveLoadService(IPersistentProgressService progressService, IStaticDataProviderService staticData)
        {
            _progressService = progressService;
            _staticData = staticData;
        }

        public event Action<PlayerProgress> OnSave;

        public void SaveProgress()
        {
            OnSave?.Invoke(_progressService.Progress);

            string progressJson = _progressService.Progress.ToJson();
            PlayerPrefs.SetString(ProgressKey, progressJson);
        }

        public PlayerProgress LoadProgress()
        {
            string progressJson = PlayerPrefs.GetString(ProgressKey);
            return progressJson?.ToDeserialized<PlayerProgress>() ?? NewProgress();
        }

        private PlayerProgress NewProgress()
        {
            PlayerProgressStaticData progressStaticData = _staticData.ForNewGame();

            var positionOnLevel =
                new PositionOnLevel(progressStaticData.PositionOnLevel.LevelName, progressStaticData.PositionOnLevel.Position);

            var playerState = new PlayerState(positionOnLevel, float.MaxValue);
            var progress = new PlayerProgress(playerState);

            return progress;
        }
    }
}