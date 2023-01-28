using CodeBase.Data;

namespace CodeBase.Services.PersistentProgress
{
    public interface ISavedProgressReader
    {
        void ReadFromProgress(PlayerProgress progress);
    }
}