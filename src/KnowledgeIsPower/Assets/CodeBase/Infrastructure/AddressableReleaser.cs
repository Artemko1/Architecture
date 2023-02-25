using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure
{
    public class AddressableReleaser : MonoBehaviour
    {
        private GameObject _prefab;

        public void Construct(GameObject addressablePrefabForRelease) =>
            _prefab = addressablePrefabForRelease;

        private void OnDestroy()
        {
            Debug.Log($"Releasing {_prefab.name}");
            Addressables.Release(_prefab);
        }
    }
}