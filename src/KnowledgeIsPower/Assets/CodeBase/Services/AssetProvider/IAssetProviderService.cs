using UnityEngine;

namespace CodeBase.Services.AssetProvider
{
    public interface IAssetProviderService : IService
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 position);
    }
}