using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Services.AssetProvider
{
    public interface IAssetProviderService : IService
    {
        Task Initialize();
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 position);
        Task<T> Load<T>(AssetReferenceT<T> assetReference) where T : Object;
        Task<T> Load<T>(string address) where T : Object;
        void Cleanup();
        Task<GameObject> InstantiateAsync(string address);
        Task<GameObject> InstantiateAsync(string address, Vector3 position);
    }
}