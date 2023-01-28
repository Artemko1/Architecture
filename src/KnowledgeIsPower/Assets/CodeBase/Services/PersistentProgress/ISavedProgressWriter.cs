using CodeBase.Data;

namespace CodeBase.Services.PersistentProgress
{
    public interface ISavedProgressWriter
    {
        void WriteToProgress(PlayerProgress progress);
    }
}