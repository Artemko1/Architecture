using System;
using CodeBase.Data;

namespace CodeBase.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveProgress();
        PlayerProgress LoadProgress();
        event Action<PlayerProgress> OnSave;
    }
}