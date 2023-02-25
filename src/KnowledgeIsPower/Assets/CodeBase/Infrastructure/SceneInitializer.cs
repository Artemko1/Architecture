using System.Linq;
using System.Threading.Tasks;
using CodeBase.Logic.Camera;
using CodeBase.Logic.Enemy.Factory;
using CodeBase.Logic.Hero;
using CodeBase.Logic.Hero.Factory;
using CodeBase.Logic.Hud;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.StaticDataProvider;
using CodeBase.StaticData;
using CodeBase.StaticData.Monsters;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class SceneInitializer : IInitializable
    {
        private readonly AssetProviderService _assetProvider;
        private readonly EnemyFactory _enemyFactory;
        private readonly HeroFactory _heroFactory;
        private readonly HudFactory _hudFactory;
        private readonly SceneLoader _sceneLoader;
        private readonly IStaticDataProviderService _staticData;
        private readonly IWarmupable[] _warmupable;

        [Inject]
        public SceneInitializer(HudFactory hudFactory, HeroFactory heroFactory, EnemyFactory enemyFactory,
            IStaticDataProviderService staticData, SceneLoader sceneLoader, IWarmupable[] warmupable, AssetProviderService assetProvider)
        {
            _hudFactory = hudFactory;
            _heroFactory = heroFactory;
            _enemyFactory = enemyFactory;
            _staticData = staticData;
            _sceneLoader = sceneLoader;
            _warmupable = warmupable;
            _assetProvider = assetProvider;
        }

        public async void Initialize()
        {
            _sceneLoader.RegisterLoading();

            await WarmupAll();
            CleanupMemory();
            await InitGameWorld();

            _sceneLoader.UnregisterLoading();
        }

        private async Task WarmupAll() =>
            await Task.WhenAll(_warmupable.Select(warmupable => warmupable.Warmup()));

        private void CleanupMemory() =>
            _assetProvider.ReleasePendingAssets();

        private async Task InitGameWorld()
        {
            LevelStaticData levelData = LevelData();

            GameObject hero = await CreateHero(levelData);
            CameraFollow(hero);

            CreateSpawners(levelData);

            await CreateHud(hero);
        }

        private Task<GameObject> CreateHero(LevelStaticData levelStaticData) =>
            _heroFactory.CreateHero(levelStaticData.InitialHeroPosition);

        private static void CameraFollow(GameObject hero) =>
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(hero);

        private void CreateSpawners(LevelStaticData levelData)
        {
            var sceneContext = Object.FindAnyObjectByType<SceneContext>();
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                _enemyFactory.CreateSpawner(spawnerData, sceneContext.transform);
            }
        }

        private LevelStaticData LevelData()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            LevelStaticData levelData = _staticData.ForLevel(sceneName);
            return levelData;
        }

        private async Task CreateHud(GameObject hero)
        {
            GameObject hud = await _hudFactory.CreateHud();
            hud.GetComponentInChildren<ActorUI>()
                .Construct(hero.GetComponent<HeroHealth>());
        }
    }
}