using CodeBase.CameraLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";
        private const string EnemySpawnerTag = "EnemySpawner";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain, IGameFactory gameFactory,
            IPersistentProgressService progressService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _gameFactory.Cleanup();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _curtain.Hide();
        }

        private void OnLoaded()
        {
            InitGameWorld();

            InformProgressReaders();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InitGameWorld()
        {
            InitSpawners();
            
            GameObject hero = InitHero();
            CameraFollow(hero);

            InitHud(hero);
        }

        private void InitSpawners()
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(EnemySpawnerTag);
            foreach (GameObject go in objectsWithTag)
            {
                var spawner = go.GetComponent<EnemySpawner>();
                _gameFactory.RegisterReader(spawner);
                _gameFactory.RegisterWriter(spawner);
            }
        }

        private GameObject InitHero() => 
            _gameFactory.CreateHero(GameObject.
                FindWithTag(InitialPointTag)
                .transform.position);

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();
            hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
        }

        private void CameraFollow(GameObject hero) =>
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(hero);

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.ReadFromProgress(_progressService.Progress);
            }
        }
    }
}