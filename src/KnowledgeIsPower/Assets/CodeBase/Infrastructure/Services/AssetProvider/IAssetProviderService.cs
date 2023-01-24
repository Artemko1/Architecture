using UnityEngine;

namespace CodeBase.Infrastructure.Services.AssetProvider
{
    public interface IAssetProviderService : IService
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 position);
    }
}