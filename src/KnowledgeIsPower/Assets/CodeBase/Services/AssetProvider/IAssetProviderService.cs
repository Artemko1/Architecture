using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Services.AssetProvider
{
    public interface IAssetProviderService : IService
    {
        Task Initialize();
        Task<T> LoadAsync<T>(AssetReferenceT<T> assetReference, bool autoTrackHandle = true) where T : Object;
        Task<T> LoadAsync<T>(string address, bool autoTrackHandle = true) where T : Object;
        void Cleanup();
    }
}