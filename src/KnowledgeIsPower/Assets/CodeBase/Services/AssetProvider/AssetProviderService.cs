using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Services.AssetProvider
{
    public class AssetProviderService
    {
        public Task<T> LoadAsync<T>(AssetReferenceT<T> assetReference) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetReference);
            return handle.Task;
        }

        public Task<T> LoadAsync<T>(string address) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            return handle.Task;
        }
    }
}