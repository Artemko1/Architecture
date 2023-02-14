using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Services.AssetProvider
{
    public interface IAssetProviderService : IService
    {
        Task Initialize();
        Task<T> Load<T>(AssetReferenceT<T> assetReference) where T : Object;
        Task<T> Load<T>(string address) where T : Object;
        void Cleanup();
    }
}