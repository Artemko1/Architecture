using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.PersistentProgress
{
    public interface ISavedProgressReader
    {
        void LoadProgress(PlayerProgress progress);
    }

    public interface ISavedProgress : ISavedProgressReader // todo segregate completely
    {
        void UpdateProgress(PlayerProgress progress);
    }
}