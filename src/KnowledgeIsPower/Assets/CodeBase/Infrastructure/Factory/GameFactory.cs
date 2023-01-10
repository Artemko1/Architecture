using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;

        public GameFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public GameObject CreateHero(GameObject initialPoint)
        {
            return _assetProvider.Instantiate(AssetPath.HeroPath, initialPoint.transform.position);
        }

        public void CreateHud()
        {
            _assetProvider.Instantiate(AssetPath.HudPath);
        }
    }
}