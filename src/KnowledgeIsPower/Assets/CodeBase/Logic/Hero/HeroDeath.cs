using System;
using CodeBase.Logic.Enemy;
using UnityEngine;

namespace CodeBase.Logic.Hero
{
    [RequireComponent(typeof(IHealth))]
    public class HeroDeath : MonoBehaviour, IDeath
    {
        [SerializeField] private GameObject _deathFx;

        private IHealth _health;
        private HeroAnimator _heroAnimator;

        private HeroAttack _heroAttack;
        private HeroMove _heroMove;

        private bool _isDead;

        private void Awake()
        {
            _heroAnimator = GetComponent<HeroAnimator>();
            _health = GetComponent<IHealth>();
            _heroAttack = GetComponent<HeroAttack>();
            _heroMove = GetComponent<HeroMove>();
        }

        private void Start() =>
            _health.HealthChanged += HealthChanged;

        private void OnDestroy() =>
            _health.HealthChanged -= HealthChanged;

        public event Action Happened;

        private void HealthChanged()
        {
            if (!_isDead && _health.Current <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _isDead = true;

            _heroMove.enabled = false;
            _heroAttack.enabled = false;
            _heroAnimator.PlayDeath();
            Instantiate(_deathFx, transform.position, Quaternion.identity);
            Happened?.Invoke();
        }
    }
}