using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Services.AssetProvider
{
    public class AssetProviderService
    {
        private readonly List<GameObject> _pendingForReleasePrefabs = new List<GameObject>();

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

        public void PendForRelease(GameObject prefab) =>
            _pendingForReleasePrefabs.Add(prefab);

        public void ReleasePendingAssets()
        {
            foreach (GameObject prefab in _pendingForReleasePrefabs)
            {
                Addressables.Release(prefab);
            }

            _pendingForReleasePrefabs.Clear();
        }
    }
}