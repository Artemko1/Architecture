using System;
using CodeBase.Logic.Enemy.Targets;
using UnityEngine;

namespace CodeBase.Logic.Enemy
{
    public interface IDeath
    {
        event Action Happened;
    }

    [RequireComponent(typeof(IHealth))]
    public class EnemyDeath : MonoBehaviour, IDeath
    {
        [SerializeField] private GameObject _deathFx;
        private EnemyAnimator _enemyAnimator;

        private IHealth _health;

        private void Awake()
        {
            _enemyAnimator = GetComponent<EnemyAnimator>();
            _health = GetComponent<IHealth>();
        }

        private void Start() =>
            _health.HealthChanged += OnHealthChanged;

        private void OnDestroy() =>
            _health.HealthChanged -= OnHealthChanged;

        public event Action Happened;

        private void OnHealthChanged()
        {
            if (_health.Current <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _health.HealthChanged -= OnHealthChanged;

            _enemyAnimator.PlayDeath();
            SpawnDeathFx();
            GetComponent<TargetNotifier>()
                .enabled = false;

            Destroy(gameObject, 4f);

            Happened?.Invoke();
        }

        private void SpawnDeathFx() =>
            Instantiate(_deathFx, transform.position, Quaternion.identity);
    }
}