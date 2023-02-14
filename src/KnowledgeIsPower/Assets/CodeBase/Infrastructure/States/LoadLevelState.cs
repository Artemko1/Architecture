using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Logic.Camera;
using CodeBase.Logic.Hero;
using CodeBase.Services.AssetProvider;
using CodeBase.Services.StaticDataProvider;
using CodeBase.StaticData;
using CodeBase.StaticData.Monsters;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IAssetProviderService _assetProvider;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly SceneLoader _sceneLoader;

        private readonly GameStateMachine _stateMachine;
        private readonly IStaticDataProviderService _staticData;
        private readonly IUIFactory _uiFactory;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain, IGameFactory gameFactory,
            IStaticDataProviderService staticData, IUIFactory uiFactory, IAssetProviderService assetProvider)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _staticData = staticData;
            _uiFactory = uiFactory;
            _assetProvider = assetProvider;
        }

        public async void Enter(string sceneName)
        {
            _curtain.Show();
            _assetProvider.Cleanup();
            await _assetProvider.Initialize();
            await _gameFactory.Warmup();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() =>
            _curtain.Hide();

        private async void OnLoaded()
        {
            await InitUIRoot();
            await InitGameWorld();

            _stateMachine.Enter<GameLoopState>();
        }

        private Task InitUIRoot() =>
            _uiFactory.CreateUIRoot();

        private async Task InitGameWorld()
        {
            LevelStaticData levelData = LevelData();

            GameObject hero = await CreateHero(levelData);
            CameraFollow(hero);

            await CreateSpawners(levelData);
            await CreateHud(hero);
        }

        private Task<GameObject> CreateHero(LevelStaticData levelStaticData) =>
            _gameFactory.CreateHero(levelStaticData.InitialHeroPosition);

        private void CameraFollow(GameObject hero) =>
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(hero);

        private async Task CreateSpawners(LevelStaticData levelData)
        {
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
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