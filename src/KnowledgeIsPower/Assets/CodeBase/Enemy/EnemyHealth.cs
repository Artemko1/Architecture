using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private float current = 5;
        [SerializeField] private float max = 5;

        private EnemyAnimator _enemyAnimator;

        private void Awake()
        {
            _enemyAnimator = GetComponent<EnemyAnimator>();
        }

        public float Current => current;
        public float Max => max;

        public event Action HealthChanged;

        public void TakeDamage(float amount)
        {
            if (current <= 0) return;

            current = Mathf.Clamp(current - amount, 0, Max);
            HealthChanged?.Invoke();

            if (current <= 0) return;

            _enemyAnimator.PlayHit();
        }
    }
}