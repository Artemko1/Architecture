using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private static GameBootstrapper _instance;
        [SerializeField] private LoadingCurtain _loadingCurtainPrefab;

        private Game _game;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this);

            _game = new Game(this, Instantiate(_loadingCurtainPrefab));
            _game.StateMachine.Enter<BootstrapState>();
        }
    }
}