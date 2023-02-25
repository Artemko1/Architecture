using System.Threading.Tasks;

namespace CodeBase.Infrastructure
{
    public interface IWarmupable
    {
        Task Warmup();
    }
}