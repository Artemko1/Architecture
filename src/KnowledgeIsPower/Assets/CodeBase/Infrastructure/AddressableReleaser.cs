using CodeBase.Services.AssetProvider;
using UnityEngine;
using UnityEngine.Assertions;

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

        private void OnDestroy()
        {
            Assert.IsNotNull(_prefab);
            _assetProviderService.PendForRelease(_prefab);
        }
    }
}