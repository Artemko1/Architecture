using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Logic.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMoveToTarget : HasTargetBehaviour
    {
        private NavMeshAgent _agent;

        private void Awake() =>
            _agent = GetComponent<NavMeshAgent>();

        private void Update()
        {
            if (Target != null)
            {
                _agent.destination = Target.position;
            }
        }

        public override void ResetTarget()
        {
            base.ResetTarget();
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