using System;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticDataProvider;
using CodeBase.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";
        private readonly PersistentProgressService _progressService;
        private readonly StaticDataProviderService _staticData;

        [Inject]
        public SaveLoadService(PersistentProgressService progressService, StaticDataProviderService staticData)
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
            LevelStaticData defaultLevel = _staticData.ForDefaultLevel();

            var positionOnLevel =
                new PositionOnLevel(defaultLevel.LevelKey, defaultLevel.InitialHeroPosition.AsVector3Data());

            var playerState = new PlayerState(positionOnLevel, float.MaxValue);
            var progress = new PlayerProgress(playerState);

            return progress;
        }
    }
}