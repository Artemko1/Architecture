using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyDeath : MonoBehaviour
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

        public event Action Happend;

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
            GetComponent<Follow>().enabled = false;
            
            Destroy(gameObject, 4f);
            
            Happend?.Invoke();
        }

        private void SpawnDeathFx() => 
            Instantiate(_deathFx, transform.position, Quaternion.identity);
    }
}