using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroHealth))]
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private GameObject _deathFx;

        private HeroAnimator _heroAnimator;
        private HeroHealth _heroHealth;
        private HeroMove _heroMove;
        private HeroAttack _heroAttack;
        private bool _isDead;

        private void Awake()
        {
            _heroHealth = GetComponent<HeroHealth>();
            _heroMove = GetComponent<HeroMove>();
            _heroAnimator = GetComponent<HeroAnimator>();
            _heroAttack = GetComponent<HeroAttack>();
        }

        private void Start() =>
            _heroHealth.HealthChanged += HealthChanged;

        private void OnDestroy() =>
            _heroHealth.HealthChanged -= HealthChanged;

        private void HealthChanged()
        {
            if (!_isDead && _heroHealth.Current <= 0)
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
        }
    }
}