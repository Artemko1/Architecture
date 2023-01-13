using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator)), RequireComponent(typeof(NavMeshAgent))]
    public class AnimateAlongAgent : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private EnemyAnimator _enemyAnimator;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _enemyAnimator = GetComponent<EnemyAnimator>();
        }

        private void Update()
        {
            _enemyAnimator.PlayMove(_agent.velocity.magnitude);
        }
    }
}