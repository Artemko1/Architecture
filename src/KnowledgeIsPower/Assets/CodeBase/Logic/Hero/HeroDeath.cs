using UnityEngine;

namespace CodeBase.Logic.Hero
{
    [RequireComponent(typeof(IHealth))]
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private GameObject _deathFx;

        private IHealth _health;

        private HeroAttack _heroAttack;
        private HeroMove _heroMove;

        private void Awake()
        {
            _health = GetComponent<IHealth>();
            _heroAttack = GetComponent<HeroAttack>();
            _heroMove = GetComponent<HeroMove>();
        }

        private void Start() =>
            _health.Died += OnDied;

        private void OnDestroy() =>
            _health.Died -= OnDied;

        private void OnDied()
        {
            _heroMove.enabled = false;
            _heroAttack.enabled = false;
            SpawnDeathFx();
        }

        private void SpawnDeathFx() =>
            Instantiate(_deathFx, transform.position, Quaternion.identity);
    }
}