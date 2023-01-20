using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
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

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgressWriter> ProgressWriters { get; } = new List<ISavedProgressWriter>();
        public GameObject HeroGameObject { get; private set; }
        public event Action HeroCreated;

        public GameObject CreateHero(Vector3 initialPoint)
        {
            HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, initialPoint);
            HeroCreated?.Invoke();
            return HeroGameObject;
        }

        public GameObject CreateHud() =>
            InstantiateRegistered(AssetPath.HudPath);

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public void RegisterWriter(ISavedProgressWriter progressUpdater) =>
            ProgressWriters.Add(progressUpdater);

        public void RegisterReader(ISavedProgressReader progressReader) =>
            ProgressReaders.Add(progressReader);

        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assetProvider.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            GameObject gameObject = _assetProvider.Instantiate(prefabPath, at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            ISavedProgressReader[] progressReaders = gameObject.GetComponentsInChildren<ISavedProgressReader>();
            foreach (ISavedProgressReader progressReader in progressReaders)
            {
                RegisterReader(progressReader);
            }

            ISavedProgressWriter[] progressUpdaters = gameObject.GetComponentsInChildren<ISavedProgressWriter>();

            foreach (ISavedProgressWriter progressUpdater in progressUpdaters)
            {
                RegisterWriter(progressUpdater);
            }
        }
    }
}