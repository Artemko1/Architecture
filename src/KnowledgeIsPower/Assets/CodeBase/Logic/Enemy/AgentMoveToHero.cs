using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Logic.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMoveToHero : Follow
    {
        private NavMeshAgent _agent;
        private Transform _heroTransform;

        private void Awake() =>
            _agent = GetComponent<NavMeshAgent>();

        private void Update()
        {
            if (_heroTransform != null)
            {
                _agent.destination = _heroTransform.position;
            }
        }

        private void OnDisable() =>
            StopAgentMove();

        public void Construct(Transform heroTransform) =>
            _heroTransform = heroTransform;

        private void StopAgentMove()
        {
            if (_agent.isOnNavMesh)
            {
                _agent.ResetPath();
            }
        }
    }
}