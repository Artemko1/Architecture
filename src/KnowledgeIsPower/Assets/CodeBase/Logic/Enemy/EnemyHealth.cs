using System;
using CodeBase.StaticData.ForComponents;
using UnityEngine;

namespace CodeBase.Logic.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private float _current = 5;

        private EnemyAnimator _enemyAnimator;
        private HealthData _healthData;

        private void Awake() =>
            _enemyAnimator = GetComponent<EnemyAnimator>();

        public float Current => _current;
        public float Max => _healthData.MaxHp;

        public event Action HealthChanged;
        public event Action Died;

        public void Construct(HealthData healthData)
        {
            _healthData = healthData;
            _current = _healthData.MaxHp;
        }

        public void TakeDamage(float amount)
        {
            if (_current <= 0 || amount <= 0) return;

            _current = Mathf.Clamp(_current - amount, 0, Max);
            HealthChanged?.Invoke();

            if (_current <= 0)
            {
                _enemyAnimator.PlayDeath();
                Died?.Invoke();
            }
            else
            {
                _enemyAnimator.PlayHit();
            }
        }
    }
}