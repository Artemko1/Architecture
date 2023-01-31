using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Logic.Enemy.Targets
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMoveToTarget : HasTargetBehaviour
    {
        private NavMeshAgent _agent;

        protected override void Awake()
        {
            base.Awake();
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (HasTarget())
            {
                _agent.destination = Target.position;
            }
        }

        protected override void OnLostTarget()
        {
            base.OnLostTarget();
            StopAgentMove();
        }

        private void StopAgentMove()
        {
            if (_agent.isOnNavMesh)
            {
                _agent.ResetPath();
            }
        }
    }
}