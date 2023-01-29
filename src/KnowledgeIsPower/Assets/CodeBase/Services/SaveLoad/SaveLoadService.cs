using System;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";
        private readonly IPersistentProgressService _progressService;

        public SaveLoadService(IPersistentProgressService progressService)
        {
            _progressService = progressService;
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
            return progressJson?.ToDeserialized<PlayerProgress>();
        }
    }
}