using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Services.AssetProvider
{
    public class AssetProviderService : IAssetProviderService
    {
        private readonly Dictionary<string, List<AsyncOperationHandle>> _allHandles = new Dictionary<string, List<AsyncOperationHandle>>();
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new Dictionary<string, AsyncOperationHandle>();

        public Task Initialize() =>
            Addressables.InitializeAsync().Task;

        public async Task<T> Load<T>(AssetReferenceT<T> assetReference) where T : Object
        {
            if (_completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
            {
                return completedHandle.Result as T;
            }

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetReference);
            return await RunWithCacheOnComplete(handle, assetReference.AssetGUID);
        }

        // LoadAssetAsync should be preffered to Addressables.InstantiateAsync as instantiation will be done by Zenject later
        public async Task<T> Load<T>(string address) where T : Object
        {
            if (_completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
            {
                return completedHandle.Result as T;
            }

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            return await RunWithCacheOnComplete(handle, address);
        }

        public void Cleanup()
        {
            foreach (AsyncOperationHandle operationHandle in _allHandles.Values.SelectMany(handles => handles))
            {
                Addressables.Release(operationHandle);
            }

            _completedCache.Clear();
            _allHandles.Clear();
        }

        private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string key) where T : Object
        {
            AddHandle(key, handle);
            T taskResult = await handle.Task;

            // all keys may and should only be stored once as each key corresponds to one addressable asset
            _completedCache.TryAdd(key, handle);
            return taskResult;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : Object
        {
            if (!_allHandles.TryGetValue(key, out List<AsyncOperationHandle> handlesOfKey))
            {
                handlesOfKey = new List<AsyncOperationHandle>();
                _allHandles.Add(key, handlesOfKey);
            }

            handlesOfKey.Add(handle);
        }
    }
}