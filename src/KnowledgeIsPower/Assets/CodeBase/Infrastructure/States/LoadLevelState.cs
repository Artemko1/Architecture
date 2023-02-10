using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Logic.Camera;
using CodeBase.Logic.Hero;
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
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly SceneLoader _sceneLoader;

        private readonly GameStateMachine _stateMachine;
        private readonly IStaticDataProviderService _staticData;
        private readonly IUIFactory _uiFactory;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain, IGameFactory gameFactory,
            IStaticDataProviderService staticData, IUIFactory uiFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _staticData = staticData;
            _uiFactory = uiFactory;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() =>
            _curtain.Hide();

        private void OnLoaded()
        {
            InitUIRoot();
            InitGameWorld();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InitUIRoot() =>
            _uiFactory.CreateUIRoot();

        private void InitGameWorld()
        {
            GameObject hero = CreateHero();
            CameraFollow(hero);

            InitSpawners();

            InitHud(hero);
        }

        private GameObject CreateHero() =>
            _gameFactory.CreateHero();

        private void CameraFollow(GameObject hero) =>
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(hero);

        private void InitSpawners()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            LevelStaticData levelData = _staticData.ForLevel(sceneName);
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
            }
        }

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();
            hud.GetComponentInChildren<ActorUI>()
                .Construct(hero.GetComponent<HeroHealth>());
        }
    }
}