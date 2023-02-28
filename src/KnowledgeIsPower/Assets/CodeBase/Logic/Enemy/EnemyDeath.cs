using CodeBase.Logic.Enemy.Targets;
using UnityEngine;

namespace CodeBase.Logic.Enemy
{
    [RequireComponent(typeof(IHealth))]
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private GameObject _deathFx;

        private IHealth _health;

        private void Awake() =>
            _health = GetComponent<IHealth>();

        private void Start() =>
            _health.Died += OnDied;

        private void OnDestroy() =>
            _health.Died -= OnDied;


        private void OnDied()
        {
            SpawnDeathFx();
            GetComponent<AggroTargetNotifier>()
                .enabled = false;

            Destroy(gameObject, 4f);
        }

        private void SpawnDeathFx() =>
            Instantiate(_deathFx, transform.position, Quaternion.identity);
    }
}