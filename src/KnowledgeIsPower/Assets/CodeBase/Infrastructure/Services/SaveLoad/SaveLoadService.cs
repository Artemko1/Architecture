using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";
        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
        }

        public void SaveProgress()
        {
            foreach (ISavedProgressWriter progressWriter in _gameFactory.ProgressWriters)
            {
                progressWriter.WriteToProgress(_progressService.Progress);
            }

            string progressJson = _progressService.Progress.ToJson();
            PlayerPrefs.SetString(ProgressKey, progressJson);
        }

        public PlayerProgress LoadProgress()
        {
            string progressJson = PlayerPrefs.GetString(ProgressKey);
            return progressJson?.ToDeserialized<PlayerProgress>();
        }
    }
}