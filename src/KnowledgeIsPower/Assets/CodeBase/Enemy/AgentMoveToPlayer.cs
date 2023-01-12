using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToPlayer : MonoBehaviour
    {
        private const float MinimalDistance = 1f;

        private NavMeshAgent _agent;
        private IGameFactory _gameFactory;
        private Transform _heroTransform;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _gameFactory = AllServices.Container.Single<IGameFactory>();


            if (_gameFactory.HeroGameObject != null)
            {
                InitializeHeroTransform();
            }
            else
            {
                _gameFactory.HeroCreated += InitializeHeroTransform;
            }
        }

        private void Update()
        {
            if (IsInitialized() && HeroNotReached())
            {
                _agent.destination = _heroTransform.position;
            }
        }

        private void OnDestroy()
            => _gameFactory.HeroCreated -= InitializeHeroTransform;

        private bool IsInitialized() =>
            _heroTransform != null;

        private void InitializeHeroTransform() =>
            _heroTransform = _gameFactory.HeroGameObject.transform;

        private bool HeroNotReached() =>
            Vector3.Distance(transform.position, _heroTransform.position) >= MinimalDistance;
    }
}