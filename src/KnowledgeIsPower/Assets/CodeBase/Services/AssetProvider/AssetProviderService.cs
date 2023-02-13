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

        public async Task<T> Load<T>(string address) where T : Object
        {
            if (_completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
            {
                return completedHandle.Result as T;
            }

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            return await RunWithCacheOnComplete(handle, address); // todo remove async and await
        }

        public GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        public GameObject Instantiate(string path, Vector3 position)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, position, Quaternion.identity);
        }

        public async Task<GameObject> InstantiateAsync(string address)
        {
            var prefab = await Load<GameObject>(address); // todo assetProvider should only provide prefab, not an instance of a prefab
            return Object.Instantiate(prefab);
        }

        public async Task<GameObject> InstantiateAsync(string address, Vector3 position)
        {
            // return await Addressables.InstantiateAsync(address, new InstantiationParameters()).Task;
            var prefab = await Load<GameObject>(address);
            return Object.Instantiate(prefab, position, Quaternion.identity);
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
            // todo await task, than return task result
            handle.Completed += operationHandle =>
                _completedCache.TryAdd(key, operationHandle);

            AddHandle(key, handle);

            return await handle.Task;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : Object
        {
            if (!_allHandles.TryGetValue(key, out List<AsyncOperationHandle> handlesOfKey))
            {
                handlesOfKey = new List<AsyncOperationHandle>();
                // _allHandles[key] = handlesOfKey;
                _allHandles.Add(key, handlesOfKey);
            }

            handlesOfKey.Add(handle);
        }
    }
}