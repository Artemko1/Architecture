using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.PersistentProgress
{
    public interface ISavedProgressWriter
    {
        void WriteToProgress(PlayerProgress progress);
    }
}