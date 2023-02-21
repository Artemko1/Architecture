using System;
using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic.Camera;
using CodeBase.Logic.Hero;
using CodeBase.Services.StaticDataProvider;
using CodeBase.StaticData;
using CodeBase.StaticData.Monsters;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure
{
    public class SceneInitializer : IInitializable, IDisposable
    {
        private readonly EnemyFactory _enemyFactory;
        private readonly GameFactory _gameFactory;
        private readonly HeroFactory _heroFactory;
        private readonly SceneLoader _sceneLoader;
        private readonly IStaticDataProviderService _staticData;
        private readonly UIFactory _uiFactory;

        // todo split this class to many
        [Inject]
        public SceneInitializer(GameFactory gameFactory, UIFactory uiFactory, HeroFactory heroFactory, EnemyFactory enemyFactory,
            IStaticDataProviderService staticData, SceneLoader sceneLoader)
        {
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _heroFactory = heroFactory;
            _enemyFactory = enemyFactory;
            _staticData = staticData;
            _sceneLoader = sceneLoader;
        }

        public void Dispose() =>
            _enemyFactory.Cleanup();

        public async void Initialize()
        {
            await _enemyFactory.Warmup();

            await InitUIRoot();
            await InitGameWorld();

            _sceneLoader.OnSceneInitializationFinish();
        }

        private Task InitUIRoot() =>
            _uiFactory.CreateUIRoot();

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

        private void CameraFollow(GameObject hero) =>
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
            GameObject hud = await _gameFactory.CreateHud();
            hud.GetComponentInChildren<ActorUI>()
                .Construct(hero.GetComponent<HeroHealth>());
        }
    }
}