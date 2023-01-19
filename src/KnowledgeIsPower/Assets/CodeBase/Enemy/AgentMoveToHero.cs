using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMoveToHero : Follow
    {
        private NavMeshAgent _agent;
        private IGameFactory _gameFactory;
        private Transform _heroTransform;

        private void Awake() =>
            _agent = GetComponent<NavMeshAgent>();

        private void Update()
        {
            if (IsInitialized())
            {
                _agent.destination = _heroTransform.position;
            }
        }

        private void OnEnable()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();

            if (IsHeroExist())
            {
                InitializeHeroTransform();
            }
            else
            {
                _gameFactory.HeroCreated += InitializeHeroTransform;
            }
        }

        private void OnDisable()
        {
            _gameFactory.HeroCreated -= InitializeHeroTransform;
            _gameFactory = null;
            StopAgentMove();
        }

        private void StopAgentMove()
        {
            if (_agent.isOnNavMesh)
            {
                _agent.ResetPath();
            }
        }

        private bool IsHeroExist() =>
            _gameFactory.HeroGameObject != null;

        private bool IsInitialized() =>
            _heroTransform != null;

        private void InitializeHeroTransform() =>
            _heroTransform = _gameFactory.HeroGameObject.transform;
    }
}