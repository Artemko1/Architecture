using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Services.AssetProvider
{
    public class AssetProviderService
    {
        private readonly Dictionary<string, List<AsyncOperationHandle>> _allTrackedHandles =
            new Dictionary<string, List<AsyncOperationHandle>>();

        public Task<T> LoadAsync<T>(AssetReferenceT<T> assetReference, bool autoTrackHandle = true) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetReference);
            if (autoTrackHandle)
            {
                AddHandle(assetReference.AssetGUID, handle);
            }

            return handle.Task;
        }

        // LoadAssetAsync should be preffered to Addressables.InstantiateAsync as instantiation will be done by Zenject later
        public Task<T> LoadAsync<T>(string address, bool autoTrackHandle = true) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            if (autoTrackHandle)
            {
                AddHandle(address, handle);
            }

            return handle.Task;
        }

        public void Cleanup()
        {
            foreach (AsyncOperationHandle operationHandle in _allTrackedHandles.Values.SelectMany(handles => handles))
            {
                Addressables.Release(operationHandle);
            }

            _allTrackedHandles.Clear();
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : Object
        {
            if (!_allTrackedHandles.TryGetValue(key, out List<AsyncOperationHandle> handlesOfKey))
            {
                handlesOfKey = new List<AsyncOperationHandle>();
                _allTrackedHandles.Add(key, handlesOfKey);
            }

            handlesOfKey.Add(handle);
        }
    }
}