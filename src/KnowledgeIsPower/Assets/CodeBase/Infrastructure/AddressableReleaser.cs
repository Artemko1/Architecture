using CodeBase.Services.AssetProvider;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class AddressableReleaser : MonoBehaviour
    {
        private AssetProviderService _assetProviderService;
        private GameObject _prefab;

        public void Construct(AssetProviderService assetProviderService, GameObject addressablePrefabForRelease)
        {
            _prefab = addressablePrefabForRelease;
            _assetProviderService = assetProviderService;
        }

        private void OnDestroy() => _assetProviderService.PendForRelease(_prefab);
        // Addressables.Release(_prefab);
    }
}