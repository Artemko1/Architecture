using System;
using UnityEngine;

namespace CodeBase.Logic.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private float _current = 5;
        [SerializeField] private float _max = 5;

        private EnemyAnimator _enemyAnimator;

        private void Awake() =>
            _enemyAnimator = GetComponent<EnemyAnimator>();

        public float Current
        {
            get => _current;
            set => _current = value;
        }

        public float Max
        {
            get => _max;
            set => _max = value;
        }

        public event Action HealthChanged;
        public event Action Died;

        public void TakeDamage(float amount)
        {
            if (_current <= 0) return;

            _current = Mathf.Clamp(_current - amount, 0, Max);
            HealthChanged?.Invoke();

            if (_current <= 0) return;

            _enemyAnimator.PlayHit();
        }
    }
}